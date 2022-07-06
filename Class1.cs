using System;
using System.Collections.Generic;
using System.Drawing;

namespace HungryHorace
{

    abstract class Object
    {
        ///
    }
    abstract class MovingObject : Object
    {
        public Map map;
        public int x;
        public int y;
        public int orientation;
        public abstract void MakeMove();

    }

    class Horace : MovingObject
    {
        public override void MakeMove()
        {
            
        }
    }

    class Ghost : MovingObject
    {
        public override void MakeMove()
        {
            
        }
    }

    enum PressedKey { none, left, right, up, down }
    enum State { notstarted, running, lost }
    class Map
    {
        private char[,] plan;
        int width;
        int height;
        public int CoinsLeft;

        public State state = State.notstarted;

        Bitmap[] icons;
        int sx; // velikost kosticky ikonek

        public Horace horace;
        public List<MovingObject> Ghosts;

        public PressedKey pressedkey;


        public Map(string pathMap, string pathIcons)
        {
            ReadIcons(pathIcons);
            ReadMap(pathMap);
            state = State.running;
        }

        public void Presun(int zX, int zY, int naX, int naY)
        {
            char c = plan[zX, zY];
            plan[zX, zY] = ' ';
            plan[naX, naY] = c;

            // podivat se, jestli tam nestal hrdina:
            if (c == 'H')
            {
                hrdina.x = naX;
                hrdina.y = naY;
                return; // kdyz na [zY,zX] stoji hrdina, tak tam nic jineho nestoji
            }

            // najit pripadny pohyblivyPrvek a zmenit mu polohu :
            foreach (PohyblivyPrvek po in PohyblivePrvkyKromeHrdiny)
            {
                if ((po.x == zX) && (po.y == zY))
                {
                    po.x = naX;
                    po.y = naY;
                    break; // jakmile tam stoji jeden, tak uz tam nikdo jiny nestoji
                }
            }

        }

        public void ZrusPohyblivyPrvek(int zX, int zY)
        {
            // najit pohyblivyPrvek a vyhodit ho ze seznamu :
            for (int i = 0; i < PohyblivePrvkyKromeHrdiny.Count; i++)
            {
                if ((PohyblivePrvkyKromeHrdiny[i].x == zX) && (PohyblivePrvkyKromeHrdiny[i].y == zY))
                {
                    PohyblivePrvkyKromeHrdiny.RemoveAt(i); // 1. vyhodit ze seznamu pohyblivych prvku...
                    plan[zX, zY] = ' ';                    // 2. ...a z planu!
                    break;
                }
            }
        }

        private (int, int) UrciPodleSmeru((int, int) pozice, string which)
        {
            int x = pozice.Item1;
            int y = pozice.Item2;


            switch ((plan[x, y]))
            {
                case '>':
                    smer = 1;
                    if (which == "right")
                        return (x + 1, y);
                    else if (which == "front")
                        return (x, y + 1);
                    break;
                case 'v':
                    smer = 2;
                    if (which == "right")
                        return (x, y - 1);
                    else if (which == "front")
                        return (x + 1, y);
                    break;
                case '^':
                    smer = 0;
                    if (which == "right")
                        return (x, y + 1);
                    else if (which == "front")
                        return (x - 1, y);
                    break;
                case '<':
                    smer = 3;
                    if (which == "right")
                        return (x - 1, y);
                    else if (which == "front")
                        return (x, y - 1);
                    break;
                default:
                    break;
            }
            return (0, 0);
        }

        public bool JeZedNapravo(int x, int y)
        {
            int i = UrciPodleSmeru((x, y), "right").Item1;
            int j = UrciPodleSmeru((x, y), "right").Item2;
            if (plan[i, j] == 'X' || smery.ContainsValue(plan[i, j]))
                return true;
            else
                return false;

        }

        public bool JeZedPredeMnou(int x, int y)
        {
            int i = UrciPodleSmeru((x, y), "front").Item1;
            int j = UrciPodleSmeru((x, y), "front").Item2;
            if (plan[i, j] == 'X' || smery.ContainsValue(plan[i, j]))
                return true;
            else
                return false;
        }

        public void Kupredu(int x, int y)
        {
            int i = UrciPodleSmeru((x, y), "front").Item1;// position in front of us 
            int j = UrciPodleSmeru((x, y), "front").Item2;//         -//-
            plan[i, j] = plan[x, y];
            plan[x, y] = '.';
            foreach (PohyblivyPrvek beast in PohyblivePrvkyKromeHrdiny)
            {
                if ((beast.x == x) && (beast.y == y))
                {
                    beast.x = i;
                    beast.y = j;
                    break; // smer zustane stejny 
                }
            }
        }

        public void Doprava(int x, int y)
        {
            plan[x, y] = smery[(smer + 1) % 4];
        }

        public void Doleva(int x, int y)
        {
            plan[x, y] = smery[(smer + 4 - 1) % 4];
        }

        public bool JeBalvan(int x, int y)
        {
            return plan[x, y] == 'B';
        }

        public bool JeHrdina(int x, int y)
        {
            return plan[x, y] == 'H';
        }

        public bool JeDiamant(int x, int y)
        {
            return plan[x, y] == 'D';
        }

        public bool JeVolno(int x, int y)
        {
            return (plan[x, y] == ' ');
        }

        public bool JeVolnoNeboHlina(int x, int y)
        {
            return (plan[x, y] == ' ') || (plan[x, y] == 'h');
        }

        public bool JeOtevrenyVychod(int x, int y)
        {
            return plan[x, y] == 'e';
        }

        public void SeberDiamant(int x, int y)
        {
            plan[x, y] = ' ';
            ZrusPohyblivyPrvek(x, y);
            ZbyvaDiamantu--;
            if (ZbyvaDiamantu == 0)
            {
                OtevriVychod();
            }

        }
        public void OtevriVychod()
        {
            for (int x = 0; x < sirka; x++)
            {
                for (int y = 0; y < vyska; y++)
                {
                    if (plan[x, y] == 'E')
                        plan[x, y] = 'e';
                }
            }

        }


        public void ReadMap(string path)
        {
            Ghosts = new List<MovingObject>();

            System.IO.StreamReader sr = new System.IO.StreamReader(cesta);
            width = int.Parse(sr.ReadLine());
            height = int.Parse(sr.ReadLine());
            plan = new char[width, height];
            CoinsLeft = 0;

            bool wasHorace = false;

            for (int y = 0; y < height; y++)
            {
                string radek = sr.ReadLine();
                for (int x = 0; x < width; x++)
                {
                    char znak = radek[x];
                    plan[x, y] = znak;

                    // vytvorit pripadne pohyblive objekty:
                    switch (znak)
                    {
                        case 'H':
                            this.hrdina = new Hrdina(this, x, y);
                            break;

                        case '<':
                        case '^':
                        case '>':
                        case 'v':
                            Prisera prisera = new Prisera(this, x, y, znak);
                            PohyblivePrvkyKromeHrdiny.Add(prisera);
                            break;

                        case 'B':
                            Balvan balvan = new Balvan(this, x, y);
                            PohyblivePrvkyKromeHrdiny.Add(balvan);
                            break;

                        case 'D':
                            Diamant diamant = new Diamant(this, x, y);
                            PohyblivePrvkyKromeHrdiny.Add(diamant);
                            ZbyvaDiamantu++;
                            break;

                        default:
                            break;
                    }
                }
            }
            sr.Close();
        }
        public void ReadIcons(string path)
        {
            Bitmap bmp = new Bitmap(path);
            this.sx = bmp.Height;
            int pocet = bmp.Width / sx; // predpokladam, ze to jsou kosticky v rade
            icons = new Bitmap[8];
            int indx = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Rectangle rect = new Rectangle(j * sx/2, i*sx/2, sx/2, sx/2);
                    icons[indx] = bmp.Clone(rect, System.Drawing.Imaging.PixelFormat.DontCare);
                    indx += 1;
                }
                
            }
        }

        public void VykresliSe(Graphics g, int sirkaVyrezuPixely, int vyskaVyrezuPixely)
        {
            int sirkaVyrezu = sirkaVyrezuPixely / sx;
            int vyskaVyrezu = vyskaVyrezuPixely / sx;

            if (sirkaVyrezu > sirka)
                sirkaVyrezu = sirka;

            if (vyskaVyrezu > vyska)
                vyskaVyrezu = vyska;

            // urcit LHR vyrezu:
            int dx = hrdina.x - sirkaVyrezu / 2;
            if (dx < 0)
                dx = 0;
            if (dx + sirkaVyrezu - 1 >= this.sirka)
                dx = this.sirka - sirkaVyrezu;

            int dy = hrdina.y - vyskaVyrezu / 2;
            if (dy < 0)
                dy = 0;
            if (dy + vyskaVyrezu - 1 >= this.vyska)
                dy = this.vyska - vyskaVyrezu;

            for (int x = 0; x < sirkaVyrezu; x++)
            {
                for (int y = 0; y < vyskaVyrezu; y++)
                {
                    int mx = dx + x; // index do mapy
                    int my = dy + y; // index do mapy

                    char c = plan[mx, my];
                    int indexObrazku = " hHB<^>vXDEe".IndexOf(c); // 0..

                    g.DrawImage(ikonky[indexObrazku], x * sx, y * sx);
                }
            }
        }

        public void PohniVsemiPrvky(PressedKey pressedkey)
        {
            this.pressedkey = pressedkey;
            foreach (PohyblivyPrvek p in PohyblivePrvkyKromeHrdiny)
            {
                p.UdelejKrok();
            }

            hrdina.UdelejKrok();
        }
    }

}


