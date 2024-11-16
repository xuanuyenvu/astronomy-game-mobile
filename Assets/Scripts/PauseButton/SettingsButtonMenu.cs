using UnityEngine ;
using UnityEngine.UI ;
using DG.Tweening ;

public class SettingsButtonMenu : MonoBehaviour 
{
   [Header ("space between menu items")]
   [SerializeField] Vector2 spacing ;

   [Space]
   [Header ("Main button rotation")]
   [SerializeField] float rotationDuration ;
   [SerializeField] Ease rotationEase ;

   [Space]
   [Header ("Animation")]
   [SerializeField] float expandDuration ;
   [SerializeField] float collapseDuration ;
   [SerializeField] Ease expandEase ;
   [SerializeField] Ease collapseEase ;

   [Space]
   [Header ("Fading")]
   [SerializeField] float expandFadeDuration ;
   [SerializeField] float collapseFadeDuration ;

   Button mainButton ;
   SettingsMenuItem[] menuItems ;

   Vector2 mainButtonPosition ;
   int itemsCount ;

   void Start () {
      //add all the items to the menuItems array
      itemsCount = transform.childCount - 1 ;
      menuItems = new SettingsMenuItem[itemsCount] ;
      for (int i = 0; i < itemsCount; i++) {
         // +1 to ignore the main button
         menuItems [ i ] = transform.GetChild (i + 1).GetComponent <SettingsMenuItem> () ;
      }

      mainButton = transform.GetChild (0).GetComponent <Button> () ;
      // mainButton.onClick.AddListener (ToggleMenu) ;
      //SetAsLastSibling () to make sure that the main button will be always at the top layer
      mainButton.transform.SetAsLastSibling () ;

      mainButtonPosition = mainButton.GetComponent <RectTransform> ().anchoredPosition ;

      //set all menu items position to mainButtonPosition
      ResetPositions() ;
   }

   private void ResetPositions () {
      for (int i = 0; i < itemsCount; i++) {
         menuItems[ i ].GetComponent<RectTransform>().anchoredPosition = mainButtonPosition ;
      }
   }

   public void ToggleMenu (bool isMenuOpened) {
      if (!isMenuOpened) {
         //menu opened
         for (int i = 0; i < itemsCount; i++) {
            menuItems[ i ].GetComponent<RectTransform>().DOAnchorPos (mainButtonPosition + spacing * (i + 1), expandDuration).SetEase (expandEase) ;
            //Fade to alpha=1 starting from alpha=0 immediately
            menuItems[ i ].GetComponent<Image>().DOFade (1f, expandFadeDuration).From (0f) ;
         }
      } else {
         //menu closed
         for (int i = 0; i < itemsCount; i++) {
            menuItems [ i ].GetComponent<RectTransform>().DOAnchorPos (mainButtonPosition, collapseDuration).SetEase (collapseEase) ;
            //Fade to alpha=0
            menuItems [ i ].GetComponent<Image>().DOFade (0f, collapseFadeDuration) ;
         }
      }
   }

   public void OnItemClick (int index) {
      //here you can add you logic 
      switch (index) {
         case 0: 
				//first button
            Debug.Log ("Music") ;
            break ;
         case 1: 
				//second button
            Debug.Log ("LoadGame") ;
            break ;
         case 2: 
				//third button
            Debug.Log ("Exit") ;
            break ;
      }
   }
}