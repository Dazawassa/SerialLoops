using ReactiveUI;

namespace SerialLoops.ViewModels.Editors
{
    public class TopicEditorViewModel : ReactiveObject
    {
        private string _topicName = "Lol";
        public string TopicName
        {
            get => _topicName;
            set => this.RaiseAndSetIfChanged(ref _topicName, value);
        }

        public TopicEditorViewModel()
        {
            TopicName = "THe ID";
        }
    }
}
