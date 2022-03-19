# Interactive_AR_E-Book
##### 簡報ppt: 
https://docs.google.com/presentation/d/1xq19L2HNOEH-LCz24NJMG7otinbAE2UH/edit?usp=sharing&ouid=102120387455669479311&rtpof=true&sd=true

### 一. 系統背景
#### 1.1 動機與目的
    一般市面上的美術設定集，能見的只有2D平面內容，而無法觀察其它細節，因此本專題認為若能以各個角度去觀察物件，將能提升其價值。
    此外，還受到紙雕書啟發，由於使用與現實結合的3D模型恰能改善此情形，因而選擇了AR為題目主軸。
    
    AR電子書除美術書外，還可以運用在其他類型的書籍上，例如繪本
    由於現今行動裝置的普及，造成兒童對於閱讀書籍的興趣低落，因此我們參考了互動式兒童節目，希望能透過故事中的語音互動遊戲，提升兒童對於閱讀的樂趣。
#### 1.2 需求工具
    主要使用五種工具，其中最核心的部分是unity遊戲引擎，它負責整合其它工具，最後形成可供我們在手機上操作的3D物件及操作介面。

    Vuforia功能則是為AR提供更為準確的圖片識別能力，透過事先對於所要偵測的物件進行輪廓上的標註，可以讓掃描更為精準。

    Blender則為AR中的主角和其他物件提供動畫設計。

    Olami語音辨識，負責將操作者發出的指令進行語意的分析，透過伺服器處理後回傳到系統，並由腳本提供後續相應的動作。

    Google cloud提供系統人聲的語音，輔以SSML文字標記，使語音呈現效果更好，此工具主要應用在語音的回覆上。

### 二. 功能設計
#### 1. 進入章節
    可以閱讀章節簡介，選擇進入指定章節，或在主菜單點擊“開始閱讀”進入起始章節
#### 2. 語音指令接收及判斷
    按住麥克風按鈕可以接收使用者的語音指令，將其導入設定好的資料庫判斷其語意，並將對應數值傳回unity
#### 3. 圖片偵測開啟ar場景
    開始閱讀章節後，將攝像頭對準指定圖片，可以開啟AR場景
#### 4. 背景聲關閉及場景角度自訂
    右下角的靜音鍵可以切換背景聲的開啟和關閉
#### 5. 劇情中加入遊戲關卡
- 選取指定選項
    在此關卡中，使用者需要點擊正確的單詞
- 選取指定數量
    在此關卡中，使用者需要收集指定數量的道具，超過數量則會重置關卡
- 選取清單指定所有項目
    在此關卡中，使用者需要將任務清單上要求的物件拖曳至指定區域
#### 6. 場景動畫及語音
    劇情推進時，場景中的角色和物件會進行對應的動作，同時根據文字內容播放語音
    章節結束之關卡結算
#### 7. 閱讀完章節後，會根據使用者在遊戲關卡的表現顯示評分
