using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MoreMountains.Tools
{
    [System.Serializable]
    public class JoystickEvent : UnityEvent<Vector2> { }
    [System.Serializable]
    public class JoystickFloatEvent : UnityEvent<float> { }

    public class MMTouchJoystick : MMMonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        public Camera targetCamera;

        [Header("Joystick")]
        // 대각선 방향 사용 여부
        public bool diagonalAxisEnabled;
        public float maxRange = 1.5f;



        [MMInspectorGroup("Value Events", true, 19)]
        /// An event to use the raw value of the joystick
        [Tooltip("An event to use the raw value of the joystick")]
        public JoystickEvent JoystickValue;
        /// An event to use the normalized value of the joystick
        [Tooltip("An event to use the normalized value of the joystick")]
        public JoystickEvent JoystickNormalizedValue;
        // An event to use the joystick's amplitude (the magnitude of its Vector2 output)
        [Tooltip("An event to use the joystick's amplitude (the magnitude of its Vector2 output)")]
        public JoystickFloatEvent JoystickMagnitudeValue;

        [MMInspectorGroup("Touch Events", true, 8)]
        /// An event triggered when tapping the joystick for the first time
        [Tooltip("An event triggered when tapping the joystick for the first time")]
        public UnityEvent OnPointerDownEvent;
        /// An event triggered when dragging the stick
        [Tooltip("An event triggered when dragging the stick")]
        public UnityEvent OnDragEvent;
        /// An event triggered when releasing the stick
        [Tooltip("An event triggered when releasing the stick")]
        public UnityEvent OnPointerUpEvent;

        [MMInspectorGroup("Rotating Direction Indicator", true, 20)]
        /// an object you can rotate to show the direction of the joystick. Will only be visible if the movement is above a threshold
        [Tooltip("an object you can rotate to show the direction of the joystick. Will only be visible if the movement is above a threshold")]
        public Transform RotatingIndicator;
        /// the threshold above which the rotating indicator will appear
        [Tooltip("the threshold above which the rotating indicator will appear")]
        public float RotatingIndicatorThreshold = 0.1f;


        //   float knobOpacity;
        //  public float pressedOpacity = 0.5f;
        // public float pressedOpacitySpeed = 1f;

        // Knob 투명도 (터치했을 때)
        public float pressedOpacity = 0.5f;
        public float pressedOpacitySpeed = 1f;




        /// the render mode of the parent canvas this stick is on
        public RenderMode ParentCanvasRenderMode { get; protected set; }

        protected Vector2 _neutralPosition;
        protected Vector2 _newTargetPosition;
        protected Vector3 _newJoystickPosition;
        protected float _initialZPosition;
        protected float _targetOpacity;
        protected CanvasGroup _canvasGroup;
        protected float _initialOpacity;
        protected Transform _knobTransform;
        protected bool _rotatingIndicatorIsNotNull = false;

        /// <summary>
        /// On Start we initialize our stick
        /// </summary>
        protected virtual void Start()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes the various parts of the stick
        /// </summary>
        /// <exception cref="Exception"></exception>
        public virtual void Initialize()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rotatingIndicatorIsNotNull = (RotatingIndicator != null);

            SetKnobTransform(this.transform);

            SetNeutralPosition();

            ParentCanvasRenderMode = GetComponentInParent<Canvas>().renderMode;
            _initialZPosition = _knobTransform.position.z;
            _initialOpacity = _canvasGroup.alpha;
        }

        /// <summary>
        /// Assigns a new transform as the joystick knob
        /// </summary>
        /// <param name="newTransform"></param>
        public virtual void SetKnobTransform(Transform newTransform)
        {
            _knobTransform = newTransform;
        }

        /// <summary>
        /// On Update we check for an orientation change if needed, and send our input values.
        /// </summary>
        protected virtual void Update()
        {
            RotateIndicator();
            HandleOpacity();
        }

        // 보간으로 opacity 조절
        protected void HandleOpacity() => _canvasGroup.alpha = Mathf.Lerp(_targetOpacity, _canvasGroup.alpha, pressedOpacitySpeed);


        /// <summary>
        /// Rotates an indicator to match the rotation of the stick
        /// </summary>
        protected virtual void RotateIndicator()
        {
            if (!_rotatingIndicatorIsNotNull)
            {
                return;
            }

        }

        /// <summary>
        /// Sets the neutral position of the joystick
        /// </summary>
        public virtual void SetNeutralPosition()
        {
            _neutralPosition = _knobTransform.position;
        }

        public virtual void SetNeutralPosition(Vector3 newPosition)
        {
            _neutralPosition = newPosition;
        }

        /// <summary>
        /// Handles dragging of the joystick
        /// </summary>
        public virtual void OnDrag(PointerEventData eventData)
        {
            OnDragEvent.Invoke();

            _newTargetPosition = ConvertToWorld(eventData.position);

            // We clamp the stick's position to let it move only inside its defined max range
            ClampToBounds();

            // 아직 안 만듬 사선 무시하는 거
            if (!diagonalAxisEnabled)
            { }
            else
            {
                _newJoystickPosition = _neutralPosition + _newTargetPosition;
                _newJoystickPosition.z = _initialZPosition;

                // We move the joystick to its dragged position
                _knobTransform.position = _newJoystickPosition;
            }
        }

        /// <summary>
        /// Clamps the stick to the specified range
        /// </summary>
        protected virtual void ClampToBounds()
        {
            _newTargetPosition = Vector2.ClampMagnitude(_newTargetPosition - _neutralPosition, maxRange);
        }

        /// <summary>
        /// Converts a position to world position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        protected virtual Vector3 ConvertToWorld(Vector3 position)
        {
            if (ParentCanvasRenderMode == RenderMode.ScreenSpaceCamera)
            {
                return targetCamera.ScreenToWorldPoint(position);
            }
            else
            {
                return position;
            }
        }

        /// <summary>
        /// Resets the stick's position and values
        /// </summary>
        public virtual void ResetJoystick()
        {
            // we reset the stick's position
            _newJoystickPosition = _neutralPosition;
            _newJoystickPosition.z = _initialZPosition;
            _knobTransform.position = _newJoystickPosition;

            // we set its opacity back
            _targetOpacity = _initialOpacity;
        }

        /// <summary>
        /// We compute the axis value from the interval between neutral position, current stick position (vectorPosition) and max range
        /// </summary>
        /// <returns>The axis value, a float between -1 and 1</returns>
        /// <param name="vectorPosition">stick position.</param>
        protected virtual float EvaluateInputValue(float vectorPosition)
        {
            return Mathf.InverseLerp(0, maxRange, Mathf.Abs(vectorPosition)) * Mathf.Sign(vectorPosition);
        }

        /// <summary>
        /// What happens when the stick stops being dragged
        /// </summary>
        public virtual void OnEndDrag(PointerEventData eventData)
        {
        }

        /// <summary>
        /// What happens when the stick is released (even if no drag happened)
        /// </summary>
        /// <param name="data"></param>
        public virtual void OnPointerUp(PointerEventData data)
        {
            ResetJoystick();
            OnPointerUpEvent.Invoke();
        }

        /// <summary>
        /// What happens when the stick is pressed for the first time
        /// </summary>
        /// <param name="data"></param>
        public virtual void OnPointerDown(PointerEventData data)
        {
            _targetOpacity = pressedOpacity;
            OnPointerDownEvent.Invoke();
        }

        /// <summary>
        /// On enable, we initialize our stick
        /// </summary>
        protected virtual void OnEnable()
        {
            Initialize();
            _targetOpacity = _initialOpacity;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Draws gizmos if needed
        /// </summary>

#endif
    }
}