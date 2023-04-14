using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Reflection;
using TMPro;

public class CopyRoomId : MonoBehaviour
{
   public TMP_Text text;
   public void onCLick()
   {
      TextEditor textEditor = new TextEditor();
      textEditor.text = text.text;
      textEditor.SelectAll();
      textEditor.Copy();
   }
}
