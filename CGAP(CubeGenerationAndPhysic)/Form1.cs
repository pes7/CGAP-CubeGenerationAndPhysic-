using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CGAP_CubeGenerationAndPhysic_
{
    public partial class Form1 : Form
    {
        Graphics Gr;
        Borders Br;
        Player Pl;

        enum GroundLevelOfBottom {Top,Middle,Bottom};
        GroundLevelOfBottom NowGroundLevel = GroundLevelOfBottom.Top;

        Size WorldSizeSettings = new Size(64,64);// Where 64 - width and 32 - sky, 32 - ground
        Size WorldShowSettings = new Size(20,4); // num / 2 to both sides, now 4 for deep, not for sky ;(

        Size RealWindowSize;

        GraphicsState Gs;

        public Form1()
        {
            InitializeComponent();
            Gr = this.CreateGraphics();
            Br.BottomLeft = new Point(-33,this.Height);
            Br.BottomRight = new Point(this.Width, this.Height);
            RealWindowSize = new Size(this.Width / Block.Size.Width,this.Height / Block.Size.Height);

            Player.BPoint bp = new Player.BPoint();
            bp.BX = 5;
            bp.BY = 4;
            Pl = new Player("Nazar", bp);
            WorldShow();
        }

        public void WorldShow()
        {
            int NowX = Br.BottomLeft.X;
            NowGroundLevel = GroundLevelOfBottom.Top;
            for (int i =  -WorldShowSettings.Height / 2; i < WorldShowSettings.Height / 2; i++)
            {
                NowX = -Block.Size.Width;
                for (int j = 0 - WorldShowSettings.Width / 2; j < WorldShowSettings.Width / 2; j++)
                {
                    Generation(new Point(NowX += Block.Size.Width, Br.BottomLeft.Y - (Block.Size.Height * (WorldShowSettings.Height / 2 - i))));
                }
                NowGroundLevel = GroundLevelOfBottom.Middle;
            }
            Gr.DrawImage(Images.Player, new Point((0 + Pl.BlockPoint.BX) * Block.Size.Width,(RealWindowSize.Height - Pl.BlockPoint.BY) * Block.Size.Height));
        }

        public void Generation(Point StartPoint)
        {
            CreateBlock(new Point(StartPoint.X , StartPoint.Y),new Size(Block.Size.Width, Block.Size.Height));
        }

        public void CreateBlock(Point po1, Size sz1)
        {
            switch (NowGroundLevel) {
                case GroundLevelOfBottom.Top:
                    Gr.DrawImage(Images.GreenTopBlock, po1);
                    break;
                case GroundLevelOfBottom.Middle:
                    Gr.DrawImage(Images.GreenMiddleBlock, po1);
                    break;
                case GroundLevelOfBottom.Bottom:

                    break;
            }
        }

        struct Borders
        {
            public Point BottomLeft;
            public Point BottomRight;
            public Point TopLeft;
            public Point TopRight;
        }

        struct Block {
            public static Size Size = new Size(32,32);
        }
        
        class Player
        {
            public string Name { get; set; }
            public BPoint BlockPoint { get; set; }

            public Player(string name, BPoint bp)
            {
                Name = name;
                BlockPoint = bp;
            }

            public struct BPoint
            {
                public int BX;
                public int BY;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Player.BPoint bp = new Player.BPoint();
            bp.BX = Pl.BlockPoint.BX;
            bp.BY = Pl.BlockPoint.BY;
            ClearPlayerLastPosition();
            switch (e.KeyValue)
            {
                case 87:

                    break;
                case 68:
                    bp.BX++;
                    Pl.BlockPoint = bp;
                    WorldShow();
                    break;
                case 65:
                    bp.BX--;
                    Pl.BlockPoint = bp;
                    WorldShow();
                    break;
                case 83:

                    break;
            }
        }
    
        public void ClearPlayerLastPosition()
        {
            Gr.FillRectangle(Brushes.White, new Rectangle(new Point((0 + Pl.BlockPoint.BX) * Block.Size.Width, (RealWindowSize.Height - Pl.BlockPoint.BY) * Block.Size.Height),new Size(32,32)));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }
    }
}
