using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace UI.classes
{
    internal class ErrorMessageManager
    {
        private List<ErrorMessage> _messages;

        public ErrorMessageManager()
            => _messages = new List<ErrorMessage>();

        public void Add(ErrorMessage message)
            => _messages.Add(message);

        public void Clear()
            => _messages.Clear();

        public List<StackPanel> Tick()
        {
            foreach (ErrorMessage message in _messages)
                message.TimeRemaining -= 1;

            List<ErrorMessage> toRemove = _messages.Where(message => message.TimeRemaining <= 0).ToList();
            _messages.RemoveAll(message => toRemove.Contains(message));
            return toRemove.Select(message => message.View).ToList();
        }
    }
}
