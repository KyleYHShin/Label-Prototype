using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NK_Label.Settings
{
    public class SystemLanguage
    {
        #region Menu

        public string MenuFile { get; set; }
        public string MenuFileNew { get; set; }
        public string MenuFileLoad { get; set; }
        public string MenuFileSave { get; set; }
        public string MenuFilePrint { get; set; }
        public string MenuFileCloseWindow { get; set; }
        public string MenuFileProgramExit { get; set; }

        public string MenuEdit { get; set; }
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
        public string MenuToolCompositeComponent { get; set; }
        public string MenuToolLine { get; set; }
        public string MenuToolImage { get; set; }

        public string MenuWindow { get; set; }
        public string MenuWindowPreference { get; set; }

        #endregion //Menu


        #region SystemMent

        public string SystemMentExit { get; set; }

        #endregion //SystemMent


        public SystemLanguage()
        {
            #region Initialize Test Source

            MenuFile = "파일";
            MenuFileNew = "새로 만들기";
            MenuFileLoad = "열기";
            MenuFileSave = "저장";
            MenuFilePrint = "인쇄";
            MenuFileCloseWindow = "창 닫기";
            MenuFileProgramExit = "종료";

            MenuEdit = "편집";
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
            MenuToolText = "텍스트";
            MenuToolBarcode = "바코드";
            MenuToolCompositeComponent = "복합 구성";
            MenuToolLine = "선";
            MenuToolImage = "이미지";

            MenuWindow = "창";
            MenuWindowPreference = "창 설정";

            SystemMentExit = "프로그램을 종료하시겠습니까?";

            #endregion // Initialize Test Source

            //Original : Load languages from specific file
        }
    }
}
