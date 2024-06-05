using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    public class LoadingUI : UIBase
    {
        /// <summary> Loading Progress Ex) 0.0f ~ 1.0f </summary>
        public float LoadingProgress
        {
            set
            {
                progressText.text = value.ToString();
                progressBar.fillAmount = value;
            }
        }


        public TMPro.TextMeshProUGUI progressText;
        public UnityEngine.UI.Image progressBar;

    }
}
