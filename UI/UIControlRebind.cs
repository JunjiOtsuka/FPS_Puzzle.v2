using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIControlRebind : MonoBehaviour
{
    [SerializeField]
    private InputActionReference _InputActionReference;

    [SerializeField]
    private bool excludeMouse = true;
    [Range(0, 10)]
    [SerializeField]
    private int selectedBinding;
    [SerializeField]
    private InputBinding.DisplayStringOptions displayStringOptions;
    [Header("Binding Info - DO NOT EDIT")]
    [SerializeField]
    private InputBinding inputBinding;
    private int bindingIndex;

    private string actionName { get; set; }

    [Header("UI Fields")]
    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Button rebindButton;
    [SerializeField]
    private Text rebindText;
    [SerializeField]
    private Button resetButton;

    private void OnEnable()
    {
        rebindButton.onClick.AddListener(() => DoRebind());
        resetButton.onClick.AddListener(() => ResetBinding());

        if(_InputActionReference != null)
        {
            RebindManager.LoadBindingOverride(_InputActionReference.action.name);
            GetBindingInfo();
            UpdateUI();
        }

        RebindManager.rebindComplete += UpdateUI;
        RebindManager.rebindCanceled += UpdateUI;
    }

    private void OnDisable()
    {
        RebindManager.rebindComplete -= UpdateUI;
        RebindManager.rebindCanceled -= UpdateUI;
    }

    private void OnValidate()
    {
        if (_InputActionReference == null)
            return; 

        GetBindingInfo();
        UpdateUI();
    }

    private void GetBindingInfo()
    {
        if (_InputActionReference.action != null)
        {
            actionName = _InputActionReference.action.name;
        }
        if(_InputActionReference.action.bindings.Count > selectedBinding)
        {
            inputBinding = _InputActionReference.action.bindings[selectedBinding];
            bindingIndex = selectedBinding;
        }
    }

    private void UpdateUI()
    {
        if (actionText != null)
            actionText.text = actionName;

        if(rebindText != null)
        {
            if (Application.isPlaying)
            {
                rebindText.text = RebindManager.GetBindingName(actionName, bindingIndex);
            }
            else
                rebindText.text = _InputActionReference.action.GetBindingDisplayString(bindingIndex);
        }
    }

    private void DoRebind()
    {
        RebindManager.StartRebind(actionName, bindingIndex, rebindText, excludeMouse);
    }

    private void ResetBinding()
    {
        RebindManager.ResetBinding(actionName, bindingIndex);
        UpdateUI();
    }
}
