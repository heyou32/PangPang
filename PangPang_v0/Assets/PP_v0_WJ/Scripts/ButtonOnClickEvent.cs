using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using MoreMountains.Tools;


    public class ButtonOnClickEvent : MonoBehaviour
    {
        public UnityEvent onClickDown;
        public UnityEvent onClickUp;
        Button button;
        void Start()
        {
            button =gameObject.GetComponentNoAlloc<Button>();
        //button.onco
        }

    }