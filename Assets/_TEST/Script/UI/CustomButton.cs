using UnityEngine.UIElements;

public class CustomButton : Button
{
    //
    // Summary:
    //     Instantiates a Button using data from a UXML file.
    public new class UxmlFactory : UxmlFactory<CustomButton, UxmlTraits> { }

    //
    // Summary:
    //     Defines UxmlTraits for the Button.
    public new class UxmlTraits : TextElement.UxmlTraits
    {
        //
        // Summary:
        //     Constructor.
        public UxmlTraits()
        {
            base.focusable.defaultValue = true;
        }
    }
}