using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsLinksScript : MonoBehaviour {

    public void BenEmail() {
        Application.OpenURL("mailto:bellob02@nyu.edu");
    }

    public void NickWebsite()
    {
        Application.OpenURL("https://ncarbonara235.wixsite.com/portfolio");
    }

    public void SnippyWebsite()
    {
        Application.OpenURL("https://thesnipster.carbonmade.com/");
    }

    public void LonnieWebsite()
    {
        Application.OpenURL("http://www.lonniejordanmusic.com/");
    }

    public void BBCLink()
    {
        Application.OpenURL("http://bbcsfx.acropolis.org.uk/?q=back+door%2C+interior%2C+open+%26+close");
    }

    public void PatrickLink()
    {
        Application.OpenURL("https://scripts.sil.org/cms/scripts/page.php?site_id=nrsi&id=OFL");
    }

    public void DumbledorLink()
    {
        Application.OpenURL("https://www.dafont.com/dumbledor.font");
    }
}
