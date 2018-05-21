namespace Program.Models
{
    public class SystemLanguage
    {
        #region Menu

        public string MenuFile { get; set; }
        public string MenuFileNew { get; set; }
        public string MenuFileLoad { get; set; }
        public string MenuFileSave { get; set; }
        public string MenuFileSaveAs { get; set; }        
        public string MenuFileSaveAll { get; set; }
        public string MenuFilePrint { get; set; }
        public string MenuFileClose { get; set; }
        public string MenuFileCloseAll { get; set; }
        public string MenuFileProgramExit { get; set; }

        public string MenuEdit { get; set; }
        public string MenuEidtDelete { get; set; }
        public string MenuEditCopy { get; set; }
        public string MenuEditTruncate { get; set; }
        public string MenuEditPaste { get; set; }
        public string MenuEditUndo { get; set; }
        public string MenuEditRedo { get; set; }

        public string MenuView { get; set; }
        public string MenuViewExpansion { get; set; }
        public string MenuViewReduce { get; set; }
        public string MenuViewGrid { get; set; }

        public string MenuTool { get; set; }
        public string MenuToolText { get; set; }
        public string MenuToolBarcode { get; set; }
        public string MenuToolRuleManager { get; set; }
        public string MenuToolLine { get; set; }
        public string MenuToolImage { get; set; }

        public string MenuWindow { get; set; }
        public string MenuWindowPreference { get; set; }

        public string MenuHelp { get; set; }
        public string MenuHelpProgInfo { get; set; }
        public string MenuHelpManual { get; set; }

        #endregion Menu


        #region System Comment

        public string SystemMentExit { get; set; }

        #endregion System Comment


        public SystemLanguage()
        {
            #region Initialize Test Source

            MenuFile = "파일";
            MenuFileNew = "새 라벨 디자인 만들기" + " (Ctrl+N)";
            MenuFileLoad = "열기" + " (Ctrl+O)";
            MenuFileSave = "저장" + " (Ctrl+S)";
            MenuFileSaveAs = "다른 이름으로 저장" + " (Ctrl+Shift+S)";
            MenuFileSaveAll = "모두 저장" + " (Ctrl+Shift+A)";
            MenuFilePrint = "인쇄" + " (Ctrl+P)";
            MenuFileClose = "디자인 창 닫기";
            MenuFileCloseAll = "디자인 창 모두 닫기";
            MenuFileProgramExit = "종료";

            MenuEdit = "편집";
            MenuEidtDelete = "선택한 객체 삭제" + " (Del)";
            MenuEditCopy = "복사";
            MenuEditTruncate = "잘라내기";
            MenuEditPaste = "붙여넣기";
            MenuEditUndo = "실행 취소";
            MenuEditRedo = "다시 실행";

            MenuView = "보기";
            MenuViewExpansion = "확대";
            MenuViewReduce = "축소";
            MenuViewGrid = "그리드";

            MenuTool = "도구";
            MenuToolText = "새 텍스트 추가" + " (Ctrl+T)";
            MenuToolBarcode = "새 바코드 추가" + " (Ctrl+B)";
            MenuToolRuleManager = "규칙 관리자" + " (Ctrl+R)";
            MenuToolLine = "선";
            MenuToolImage = "이미지";

            MenuWindow = "창";
            MenuWindowPreference = "창 설정";

            MenuHelp = "도움말";
            MenuHelpProgInfo = "프로그램 정보";
            MenuHelpManual = "메뉴얼";

            SystemMentExit = "프로그램을 종료하시겠습니까?";

            #endregion Initialize Test Source
        }
    }
}
