using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public sealed class CustomSlider : UIBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

     private const string ANIMATOR_DRAG_PARAM = "Dragging";

     public RectTransform fillRect;
     public bool openOnStart = false;
     public bool isOpen { get; private set; }

     private RectTransform canvasRect, rectTransform;
     private Animator animator;
     private float fillHeight, minPosY, maxPosY, imageHeight, targetPosY;
     private bool isDragging, draggingDown;

         protected override void Start() {
              Canvas canvas = GetComponentInParent<Canvas>();
              this.canvasRect = canvas.transform as RectTransform;

              this.rectTransform = this.transform as RectTransform;
              this.imageHeight = this.rectTransform.sizeDelta.y;

              Vector3[] fillCorners = new Vector3[4];
              this.fillRect.GetLocalCorners(fillCorners);
              this.fillHeight = Mathf.Abs(fillCorners[0].y * 2f);
              this.maxPosY = fillCorners[0].y + this.imageHeight / 2f;
              this.minPosY = fillCorners[1].y - this.imageHeight / 2f;

              if (this.openOnStart) Open();
              else Close();
              this.animator = GetComponent<Animator>();
         }

     private void Update() {
          UpdateFill();
          UpdatePosition();

          this.animator.SetBool(ANIMATOR_DRAG_PARAM, this.isDragging);
     }

     private void UpdatePosition() {
          Vector2 currentPos = this.rectTransform.localPosition;
          if (this.isDragging == false) {
           float newYPos = (this.isOpen) ? this.maxPosY : this.minPosY;
           float speed = Time.deltaTime * 10f;
           this.targetPosY = Mathf.Lerp(currentPos.y, newYPos, speed);
          }
          float yPos = Mathf.Clamp(this.targetPosY, this.maxPosY, this.minPosY);
          this.rectTransform.localPosition = new Vector2(currentPos.x, yPos);

          float percentComplete = GetFillValue() * 100f;
          if (percentComplete > 95f) this.rectTransform.rotation = Quaternion.Euler(0, 0, 180);
          else this.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
     }

     private void UpdateFill() {
          float value = GetFillValue();
          float newSizeY = this.fillHeight * value;
          RectTransform.Edge edge = RectTransform.Edge.Top;
          this.fillRect.SetInsetAndSizeFromParentEdge(edge, 0f, newSizeY);
     }

     public void OnBeginDrag(PointerEventData eventData) {
         this.isDragging = true;   
     }

     public void OnDrag(PointerEventData eventData) {
          Camera eventCam = eventData.pressEventCamera;
          Vector2 worldPoint = eventCam.ScreenToWorldPoint(eventData.position);
          Vector2 localPoint = this.canvasRect.InverseTransformPoint(worldPoint);
          this.draggingDown = localPoint.y < this.targetPosY;
          this.targetPosY = localPoint.y;
     }

     public void OnEndDrag(PointerEventData eventData) {
          this.isDragging = false;

          float percentComplete = GetFillValue() * 100f;
          if (percentComplete > 10f && this.draggingDown) Open();
          else if (this.draggingDown == false) Close();
     }

     private float GetFillValue() {
          float currentYPos = this.rectTransform.localPosition.y;
          float diff = currentYPos - this.minPosY;
          float result = -(diff / (this.fillHeight - this.imageHeight));
          return result;
     }

     public void Open() {
        if (this.isOpen == false && this.isDragging == false) this.isOpen = true;
     }

     public void Close() {
        if (this.isOpen && this.isDragging == false) this.isOpen = false;
     }

}