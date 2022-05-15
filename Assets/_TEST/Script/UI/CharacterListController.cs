using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterListController
{
    List<CharacterData> allCharacters;

    // UXML template for list entries
    VisualTreeAsset listEntryTemplate;

    // UI element references
    ListView characterList;
    Label charClassLabel;
    Label charNameLabel;
    VisualElement charPortrait;
    Button selectCharButton;

    public void InitializeCharacterList(VisualElement root, VisualTreeAsset listElementTemplate, CharacterData[] characterDatas)
    {
        allCharacters = new List<CharacterData>();
        allCharacters.AddRange(characterDatas);

        // Store a reference to the template for the list entries
        listEntryTemplate = listElementTemplate;

        // Store a reference to the character list element
        characterList = root.Q<ListView>("CharacterList");

        // Store references to the selected character info elements
        charClassLabel = root.Q<Label>("CharacterClass");
        charNameLabel = root.Q<Label>("CharacterName");
        charPortrait = root.Q<VisualElement>("CharacterPortrait");

        // Store a reference to the select button
        selectCharButton = root.Q<Button>("SelectCharButton");

        FillCharacterList();

        // Register to get a callback when an item is selected
        characterList.onSelectionChange += OnCharacterSelected;
    }

    void FillCharacterList()
    {
        // Set up a make item function for a list entry
        characterList.makeItem = () =>
        {
            // Instantiate the UXML template for the entry
            var newListEntry = listEntryTemplate.Instantiate();

            // Instantiate a controller for the data
            var newListEntryLogic = new CharacterListEntryController();

            // Assign the controller script to the visual element
            newListEntry.userData = newListEntryLogic;

            // Initialize the controller script
            newListEntryLogic.SetVisualElement(newListEntry);

            // Return the root of the instantiated visual tree
            return newListEntry;
        };

        // Set up bind function for a specific list entry
        characterList.bindItem = (item, index) =>
        {
            (item.userData as CharacterListEntryController).SetCharacterData(allCharacters[index]);
        };

        // Set a fixed item height
        characterList.fixedItemHeight = 45;

        // Set the actual item's source list/array
        characterList.itemsSource = allCharacters;
    }

    void OnCharacterSelected(IEnumerable<object> selectedItems)
    {
        // Get the currently selected item directly from the ListView
        var selectedCharacter = characterList.selectedItem as CharacterData;

        // Handle none-selection (Escape to deselect everything)
        if (selectedCharacter == null)
        {
            // Clear
            charClassLabel.text = "";
            charNameLabel.text = "";
            charPortrait.style.backgroundImage = null;

            // Disable the select button
            selectCharButton.SetEnabled(false);

            return;
        }

        // Fill in character details
        charClassLabel.text = selectedCharacter.m_Class.ToString();
        charNameLabel.text = selectedCharacter.m_CharacterName;
        charPortrait.style.backgroundImage = new StyleBackground(selectedCharacter.m_PortraitImage);

        // Enable the select button
        selectCharButton.SetEnabled(true);
    }
}