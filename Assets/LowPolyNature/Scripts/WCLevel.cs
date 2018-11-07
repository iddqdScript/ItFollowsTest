using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WCLevel : MonoBehaviour
{

    public Image ImgHealthBar;

    public Text TxtWCLevel;

    public int Min;

    public int Max;

    private int mCurrentValue = 12;

    private float mCurrentPercent;

    public void SetValue(int WCLevel)
    {
        Debug.Log("We in here");
        TxtWCLevel.text = string.Format("12");

        //if (WCLevel != mCurrentValue)
        //{
        //    if (Max - Min == 0)
        //    {
        //        mCurrentValue = 0;
        //        mCurrentPercent = 0;
        //    }
        //    else
        //    {
        //        mCurrentValue = WCLevel;
        //        mCurrentPercent = (float)mCurrentValue / (float)(Max - Min);
        //    }

        //    TxtHealth.text = string.Format("{0} %", Mathf.RoundToInt(mCurrentPercent * 100));

        //    ImgHealthBar.fillAmount = mCurrentPercent;
        //}
    }

}
