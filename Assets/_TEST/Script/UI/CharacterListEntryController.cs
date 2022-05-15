using UnityEngine.UIElements;

public class CharacterListEntryController
{
    Label m_NameLabel;

    /// <summary>
    /// This function will receive a visual element that is an instance of the ListEntry UI template you created in the previous section.
    /// The main view controller will create this instance.
    /// The purpose of this function is to retrieve a reference to the character name label inside the UI element.
    /// </summary>
    /// <param name="visualElement"></param>
    public void SetVisualElement(VisualElement visualElement)
    {
        m_NameLabel = visualElement.Q<Label>("CharacterName");
    }

    /// <summary>
    ///  This function receives the character whose name this list element is supposed to display.
    ///  Because the elements list in a <see cref="ListView"></see> are pooled and reused,
    ///  it’s necessary to have a Set function to change which character’s data to display.
    /// </summary>
    /// <param name="characterData"></param>
    public void SetCharacterData(CharacterData characterData)
    {
        m_NameLabel.text = characterData.m_CharacterName;
    }
}