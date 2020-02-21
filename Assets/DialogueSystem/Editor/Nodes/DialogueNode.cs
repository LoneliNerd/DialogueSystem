﻿using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

/*
 * TODO: Note for building UI
 * If a DialogueNode has no ports OR if the port has no edges, display option to end conversation.
 */
namespace lastmilegames.DialogueSystem.Nodes
{
    class DialogueNode : BaseNode
    {
        public string DialogueText { get; set; }
        public string SpeakerName { get; set; }

        public DialogueNode(Action<Node, Port> onClickRemovePort) : base(onClickRemovePort)
        {
            // Associate stylesheet
            styleSheets.Add(Resources.Load<StyleSheet>("Node"));

            // Add node fields
            BuildNodeControls();
        }
        
        private void BuildNodeControls()
        {
            Button addChoiceButton = new Button(AddChoicePort) {text = "Add Choice"};
            titleButtonContainer.Add(addChoiceButton);
            
            TextField speakerNameField = new TextField("Speaker Name");
            speakerNameField.RegisterValueChangedCallback(evt =>
            {
                SpeakerName = evt.newValue;
                UpdateTitle();
            });
            contentContainer.Add(speakerNameField);
            contentContainer.Add(new VisualElement {name = "divider"});

            TextField dialogueTextField = new TextField("Dialogue Text");
            dialogueTextField.RegisterValueChangedCallback(evt =>
            {
                DialogueText = evt.newValue;
                UpdateTitle();
            });
            contentContainer.Add(dialogueTextField);
            contentContainer.Add(new VisualElement {name = "divider"});

            RefreshExpandedState();
            RefreshPorts();
            SetPosition(new Rect(Vector2.zero, DefaultNodeSize));
        }

        private void AddChoicePort()
        {
            Port port = this.GeneratePort(this,"Out", Direction.Output, type:typeof(string));
            
            TextField choiceText = new TextField
            {
                name = string.Empty,
                value = "Choice Text"
            };
            port.contentContainer.Add(choiceText);
            
            Button deleteButton = new Button(() => OnClickRemovePort?.Invoke(this, port))
            {
                text = "X"
            };
            port.contentContainer.Add(deleteButton);
            
            outputContainer.Add(port);
            RefreshExpandedState();
            RefreshPorts();
        }

        private void UpdateTitle()
        {
            if (string.IsNullOrWhiteSpace(SpeakerName) && string.IsNullOrWhiteSpace(DialogueText))
            {
                title = "Dialogue";
            }
            else
            {
                string speakerName;
                if (string.IsNullOrWhiteSpace(SpeakerName))
                {
                    speakerName = "NO-SPK"; 
                }
                else
                {
                    speakerName = SpeakerName.Length <= 10 
                        ? SpeakerName 
                        : SpeakerName.Substring(0, 10) + "...";
                }

                string dialogueText;
                if (string.IsNullOrWhiteSpace(DialogueText))
                {
                    dialogueText = "NO-DLG";
                }
                else
                {
                    dialogueText = DialogueText.Length <= 12
                        ? DialogueText
                        : DialogueText.Substring(0, 12) + "...";
                }
                
                title = $"{speakerName} : {dialogueText}";
            }
        }
    }
}