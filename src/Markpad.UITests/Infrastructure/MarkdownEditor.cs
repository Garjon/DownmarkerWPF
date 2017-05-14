using System.Threading;
using System.Windows.Automation;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.Utility;
using TestStack.White.WindowsAPI;

namespace MarkPad.UITests.Infrastructure
{
    public class MarkdownEditor : ScreenComponent<MarkpadWindow>
    {
        public MarkdownEditor(MarkpadWindow parentScreen, IUIItem editorControl) : base(parentScreen)
        {
            EditorUIItem = editorControl;
        }

        public IUIItem EditorUIItem { get; private set; }

        public string MarkdownText
        {
            get
            {
                var valuePattern = (ValuePattern)EditorUIItem.AutomationElement.GetCurrentPattern(ValuePattern.Pattern);
                return valuePattern.Current.Value;
            }
            set
            {
                var valuePattern = (ValuePattern)EditorUIItem.AutomationElement.GetCurrentPattern(ValuePattern.Pattern);
                valuePattern.SetValue(value);
            }
        }

        public void TypeText(string list)
        {
            if (!EditorUIItem.IsFocussed)
                EditorUIItem.Focus();

            ParentScreen.WhiteWindow.Keyboard.Enter(list);
            Retry.ForDefault(() => MarkdownText, s => !s.Contains(list));
        }

        public void PressKey(KeyboardInput.SpecialKeys @return)
        {
            ParentScreen.WhiteWindow.Keyboard.PressSpecialKey(@return);
        }

        public void MoveCursorToEndOfEditor()
        {
            ParentScreen.WhiteWindow.Keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
            ParentScreen.WhiteWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.END);
            ParentScreen.WhiteWindow.Keyboard.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
        }

        public void SelectAllText()
        {
            if (!EditorUIItem.IsFocussed)
                EditorUIItem.Focus();

            ParentScreen.WhiteWindow.Keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
            ParentScreen.WhiteWindow.Keyboard.Enter("A");
            ParentScreen.WhiteWindow.Keyboard.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
        }

        public void UnselectText(int numberOfCharacters)
        {
            if (!EditorUIItem.IsFocussed)
                EditorUIItem.Focus();

            Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.SHIFT);

            for (int i = 0; i < numberOfCharacters; i++)
            {
                ParentScreen.WhiteWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.LEFT);
            }

            Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.SHIFT);
        }

        public void PressButton(string buttonName)
        {
            ParentScreen.WhiteWindow.Get<Button>(buttonName).Click();
        }

        public bool IsControlVisible(string controlName)
        {
            return ParentScreen.WhiteWindow.Get(SearchCriteria.ByAutomationId(controlName)).Visible;
        }
    }
}