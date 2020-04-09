using UnityEngine;
using UnityEngine.Events;


namespace TWizard.Utils
{
    public class VersionText : MonoBehaviour
    {
        protected class SetTextEvent : UnityEvent<string> { }

        [SerializeField]
        private string format = "v. {0}";

        [SerializeField]
        private SetTextEvent setText = default;
        public event UnityAction<string> SetText
        {
            add => setText.AddListener(value);
            remove => setText.RemoveListener(value);
        }

        private void Start()
        {
            setText.Invoke(string.Format(format, Application.version));
        }
    }
}
