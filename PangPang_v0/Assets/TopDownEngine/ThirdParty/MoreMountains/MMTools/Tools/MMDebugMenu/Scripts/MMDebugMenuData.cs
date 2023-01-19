using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif

namespace MoreMountains.Tools
{
	/// <summary>
	/// A class used to store and display a reorderable list of menu items
	/// </summary>
	[Serializable]
	public class MMDebugMenuItemList : MMReorderableArray<MMDebugMenuItem>
	{

	}

	[Serializable]
	public class MMDebugMenuTabData
	{
		public string Name = "TabName";
		public bool Active = true;
		[MMReorderableAttribute]
		public MMDebugMenuItemList MenuItems;
	}

	/// <summary>
	/// A class used to store a menu item
	/// </summary>
	[Serializable]
	public class MMDebugMenuItem
	{
		// EDITOR NAME
		public string Name;
		public bool Active = true;
		public enum MMDebugMenuItemTypes { Title, Spacer, Button, Checkbox, Slider, Text, Value, Choices }

		public MMDebugMenuItemTypes Type = MMDebugMenuItemTypes.Title;

		// TITLE
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Title)]
		public string TitleText = "Title text";

		// TEXT
		public enum MMDebugMenuItemTextTypes { Tiny, Small, Long }
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Text)]
		public MMDebugMenuItemTextTypes TextType = MMDebugMenuItemTextTypes.Tiny;
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Text)]
		public string TextContents = "Lorem ipsum dolor sit amet";

		// CHOICES 
		public enum MMDebugMenuItemChoicesTypes { TwoChoices, ThreeChoices }
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Choices)]
		public MMDebugMenuItemChoicesTypes ChoicesType = MMDebugMenuItemChoicesTypes.TwoChoices;
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Choices)]
		public string ChoiceOneText;
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Choices)]
		public string ChoiceOneEventName = "ChoiceOneEvent";
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Choices)]
		public string ChoiceTwoText;
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Choices)]
		public string ChoiceTwoEventName = "ChoiceTwoEvent";
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Choices)]
		public string ChoiceThreeText;
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Choices)]
		public string ChoiceThreeEventName = "ChoiceThreeEvent";
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Choices)]
		public int SelectedChoice = 0;

		// VALUE
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Value)]
		public string ValueLabel = "Value Label";
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Value)]
		public string ValueInitialValue = "255";
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Value)]
		public int ValueMMRadioReceiverChannel = 0;

		// BUTTON
		public enum MMDebugMenuItemButtonTypes { Border, Full }
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Button)]
		public string ButtonText = "Button text";
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Button)]
		public MMDebugMenuItemButtonTypes ButtonType = MMDebugMenuItemButtonTypes.Border;
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Button)]
		public string ButtonEventName = "Button";

		// SPACER
		public enum MMDebugMenuItemSpacerTypes { Small, Big }
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Spacer)]
		public MMDebugMenuItemSpacerTypes SpacerType = MMDebugMenuItemSpacerTypes.Small;

		// CHECKBOX
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Checkbox)]
		public string CheckboxText;
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Checkbox)]
		public bool CheckboxInitialState = false;
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Checkbox)]
		public string CheckboxEventName = "CheckboxEventName";

		// SLIDER
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Slider)]
		public MMDebugMenuItemSlider.Modes SliderMode = MMDebugMenuItemSlider.Modes.Float;
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Slider)]
		public string SliderText;
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Slider)]
		public float SliderRemapZero = 0f;
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Slider)]
		public float SliderRemapOne = 1f;
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Slider)]
		public float SliderInitialValue = 0f;
		[EnumCondition("Type", (int)MMDebugMenuItemTypes.Slider)]
		public string SliderEventName = "Slider";

		[MMHidden]
		public MMDebugMenuItemSlider TargetSlider;
		[MMHidden]
		public MMDebugMenuItemButton TargetButton;
		[MMHidden]
		public MMDebugMenuItemCheckbox TargetCheckbox;
	}

	/// <summary>
	/// A data class used to store the contents of a debug menu
	/// </summary>
	[CreateAssetMenu(fileName = "MMDebugMenuData", menuName = "MoreMountains/MMDebugMenu/MMDebugMenuData")]
	public class MMDebugMenuData : ScriptableObject
	{
		[Header("Prefabs")]
		public MMDebugMenuItemTitle TitlePrefab;
		public MMDebugMenuItemButton ButtonPrefab;
		public MMDebugMenuItemButton ButtonBorderPrefab;
		public MMDebugMenuItemCheckbox CheckboxPrefab;
		public MMDebugMenuItemSlider SliderPrefab;
		public GameObject SpacerSmallPrefab;
		public GameObject SpacerBigPrefab;
		public MMDebugMenuItemText TextTinyPrefab;
		public MMDebugMenuItemText TextSmallPrefab;
		public MMDebugMenuItemText TextLongPrefab;
		public MMDebugMenuItemValue ValuePrefab;
		public MMDebugMenuItemChoices TwoChoicesPrefab;
		public MMDebugMenuItemChoices ThreeChoicesPrefab;
		public MMDebugMenuTab TabPrefab;
		public MMDebugMenuTabContents TabContentsPrefab;
		public RectTransform TabSpacerPrefab;
		public MMDebugMenuDebugTab DebugTabPrefab;
		public string DebugTabName = "Logs";

		[Header("Tabs")]
		public List<MMDebugMenuTabData> Tabs;
		public bool DisplayDebugTab = true;
		public int MaxTabs = 5;
		public int InitialActiveTabIndex = 0;
        
		[Header("Toggle")]
		public MMDebugMenu.ToggleDirections ToggleDirection = MMDebugMenu.ToggleDirections.RightToLeft;
		public float ToggleDuration = 0.2f;
		public MMTween.MMTweenCurve ToggleCurve = MMTween.MMTweenCurve.EaseInCubic;
        
		#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
			public Key ToggleKey = Key.Backquote;
		#else
		public KeyCode ToggleShortcut = KeyCode.Quote;
		#endif

		[Header("Style")]
		public Font RegularFont;
		public Font BoldFont;
		public Color BackgroundColor = Color.black;
		public Color AccentColor = MMColors.ReunoYellow;
		public Color TextColor = Color.white;
	}
}