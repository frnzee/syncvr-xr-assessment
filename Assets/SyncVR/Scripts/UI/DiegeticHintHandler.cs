using System.Collections;
using SyncVR.Scripts.Core;
using TMPro;
using UnityEngine;

namespace SyncVR.Scripts.UI
{
    public class DiegeticHintHandler : MonoBehaviour
    {
        private const float DisplayDuration = 3f;

        private static readonly string[] WrongOrderHints =
        {
            "Pick up the ingot from the table!",
            "Heat the ingot in the forge!",
            "Place the heated ingot on the anvil and hammer it!",
            "Quench the blade in the water pot!",
            "Grind the blade on the sharpening stone!",
            "Place the finished blade on the rack!"
        };

        [SerializeField] private BlacksmithWorkflowSystem _workflowSystem;
        [SerializeField] private TMP_Text _hintText;
        [SerializeField] private GameObject _hintRoot;

        private Coroutine _hideCoroutine;

        private void Awake()
        {
            _workflowSystem.OnWrongStepAttempted += OnWrongStepAttempted;
            _hintRoot.SetActive(false);
        }

        private void OnDestroy()
        {
            _workflowSystem.OnWrongStepAttempted -= OnWrongStepAttempted;
        }

        public void ShowHint(string message)
        {
            _hintText.text = message;
            _hintRoot.SetActive(true);

            if (_hideCoroutine != null)
                StopCoroutine(_hideCoroutine);

            _hideCoroutine = StartCoroutine(HideAfterDelay());
        }

        private void OnWrongStepAttempted(int currentStep)
        {
            if (currentStep >= WrongOrderHints.Length) return;

            ShowHint(WrongOrderHints[currentStep]);
        }

        private IEnumerator HideAfterDelay()
        {
            yield return new WaitForSeconds(DisplayDuration);
            _hintRoot.SetActive(false);
        }
    }
}