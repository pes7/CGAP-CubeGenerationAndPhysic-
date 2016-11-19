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

        public enum GroundLevelOfBottom {Top,Middle,Bottom};
        public enum GSide { Left, Right, Top, Bottom };
        public enum BType { GrassTop, DritBottom, DirtBottom, Space };
        GroundLevelOfBottom NowGroundLevel = GroundLevelOfBottom.Top;

        Size WorldSizeSettings = new Size(64,64);// Where 64 - width and 32 - sky, 32 - ground
        Size WorldShowSettings = new Size(20,4); // num / 2 to both sides, now 4 for deep, not for sky ;(

        Size RealWindowSize;

        static Map GMap;

        public Form1()
        {
            InitializeComponent();
            Gr = this.CreateGraphics();
            Br.BottomLeft = new Point(-33,this.Height);
            Br.BottomRight = new Point(this.Width, this.Height);
            RealWindowSize = new Size(this.Width / Block.Size.Width,this.Height / Block.Size.Height);

            Player.BPoint bp = new Player.BPoint();
            bp.BX = 5;
            bp.BY = 0;
            Pl = new Player("Nazar", bp);
            ExperimentalGeneration();
            Console.WriteLine("Generation Complited!");
            WorldShow();
        }

        public void WorldShow()
        {
            int NowX = Br.BottomLeft.X;
            for (int i = 0; i < WorldShowSettings.Height; i++)
            {
                NowX = -Block.Size.Width;
                for (int j = 0; j < WorldShowSettings.Width; j++)
                {
                    Gr.DrawImage(GMap.LLevel[i].LMBlock[j].Bitmap, new Point(NowX += Block.Size.Width, Br.BottomLeft.Y - (Block.Size.Height * (WorldShowSettings.Height - i))));
                }
            }
            Gr.DrawImage(Images.Player, new Point((0 + Pl.BlockPoint.BX) * Block.Size.Width, (RealWindowSize.Height - WorldShowSettings.Height - Pl.BlockPoint.BY) * Block.Size.Height));
        }

        public void ExperimentalGeneration()
        {
            List<Level> lv = new List<Level>();
            GMap = new Map(lv);
            NowGroundLevel = GroundLevelOfBottom.Top;
            for (int i = 0; i < WorldSizeSettings.Height; i++)
            {
                List<MBlock> mb = new List<MBlock>();
                for (int j = 0;j < WorldSizeSettings.Width; j++)
                {
                    mb.Add(new MBlock(new Size(32, 32), j , (BType)NowGroundLevel, CreateBlock()));
                    //Console.WriteLine($"BT: {(BType)NowGroundLevel} X: {j} Y: {i} GN!");
                }
                GMap.LLevel.Add(new Level(mb, i , mb.Count));
                NowGroundLevel = GroundLevelOfBottom.Middle;
            }
        }

        
        public bool CheckColusion(GSide Side)
        {
            switch (Side) {
                case GSide.Left:
                    if(GMap.LLevel[Pl.BlockPoint.BY].LMBlock[Pl.BlockPoint.BX - 1].Type == BType.DritBottom)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                default:
                    return true;
                }
        }

        public void Generation(Point StartPoint)
        {
           // CreateBlock(new Point(StartPoint.X , StartPoint.Y),new Size(Block.Size.Width, Block.Size.Height));
        }

        public Bitmap CreateBlock()
        {
            switch (NowGroundLevel) {
                case GroundLevelOfBottom.Top:
                    return Images.GreenTopBlock;
                case GroundLevelOfBottom.Middle:
                    return Images.GreenMiddleBlock;
                case GroundLevelOfBottom.Bottom:
                    return null;
                default:
                    return null;
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
        
        class Map {
            public List<Level> LLevel { get; set; }
            public Map(List<Level> llevel)
            {
                LLevel = llevel;
            }
        } 

        class Level {
            public int Count { get; set; }
            public int Y { get; set; }
            public List<MBlock> LMBlock { get; set; }
            public Level(List<MBlock> lmblock,int y,int count = 0)
            {
                Count = count;
                Y = y;
                LMBlock = lmblock;
            }
        }

        class MBlock
        {
            public Size Size { get; set; }
            public int X { get; set; }
            public BType Type { get; set; }
            public int IsVisable { get; set; }
            public Bitmap Bitmap { get; set; }
            public MBlock(Size size,int x, BType type,Bitmap im,int isvisable = 1)
            {
                X = x;
                Size = size;
                Type = type;
                Bitmap = im;
                IsVisable = isvisable;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Player.BPoint bp = new Player.BPoint();
            bp.BX = Pl.BlockPoint.BX;
            bp.BY = Pl.BlockPoint.BY;
            switch (e.KeyValue)
            {
                case 87:

                    break;
                case 68:
                    ClearPlayerLastPosition();
                    bp.BX++;
                    Pl.BlockPoint = bp;
                    WorldShow();
                    break;
                case 65:
                    if (CheckColusion(GSide.Left))
                    {
                        ClearPlayerLastPosition();
                        bp.BX--;
                        Pl.BlockPoint = bp;
                        WorldShow();
                    }
                    break;
                case 83:

                    break;
            }
        }
    
        public void ClearPlayerLastPosition()
        {
            Gr.FillRectangle(Brushes.White, new Rectangle(new Point((0 + Pl.BlockPoint.BX) * Block.Size.Width, (RealWindowSize.Height - WorldShowSettings.Height - Pl.BlockPoint.BY) * Block.Size.Height),new Size(32,32)));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }
    }
}
