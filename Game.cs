﻿using System;
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

    enum PressedKey { none, left, right, up, down }

    class Horace : MovingObject
    {
        public Horace(Map hmapa, int hx, int hy)
        {
            this.map = hmapa;
            this.x = hx;
            this.y = hy;

        }
        public override void MakeMove()
        {
            int newx = x;
            int newy = y;

            switch (map.pressedkey)
            {
                case PressedKey.none:
                    break;
                case PressedKey.left:
                    newx -=1;
                    break;
                case PressedKey.right:
                    newx +=1;
                    break;
                case PressedKey.up:
                    newy -=1;
                    break;
                case PressedKey.down:
                    newy +=1;
                    break;
                default:
                    break;
            }

            if (map.IsEmptyHorace(newx, newy))
            {
                if (map.CheckCell(newx, newy, 'C'))
                    map.EatCoin(newx, newy);
                map.Move(x, y, newx, newy);
            }
            else if (map.IsOpenDoor(newx, newy))
            {
                map.Move(x, y, newx, newy);
                map.state = State.win;
            }
            
        }
    }

    class Ghost : MovingObject
    {
        public int prevx;
        public int prevy;
        public Map.GhostState state;

        public Ghost(Map gmap, int gx, int gy)
        {
            this.map = gmap;
            this.x = gx;
            this.y = gy;
            this.prevx = gx;
            this.prevy = gy;
            this.state = Map.GhostState.chill;
            ///this.orientation 
        }

        public override void MakeMove()
        {
            //map.GhostAI(map.horace.x, map.horace.y, x, y);
            map.ChoseNextCell(this,x, y, state);
            if (map.CheckCell(map.nextx, map.nexty, 'H'))
            {
                
                map.Move(x, y, map.nextx, map.nexty);
                map.state = State.lost;
            }
            else if (map.IsEmptyGhost(map.nextx, map.nexty))
            {
                map.Move(x, y, map.nextx, map.nexty);
            }
            
        }
    }

    enum State { notstarted, running, lost, win}

    class Map
    {
        private char[,] plan;
        int width;
        int height;
        public int coinsLeft;
        public bool pill;
        private int[] door = new int[4];
        public bool[,] coinsplan;

        public State state = State.notstarted;

        Bitmap[] icons;
        int sx; // velikost kosticky ikonek

        public Horace horace;
        public List<MovingObject> Ghosts;

        public PressedKey pressedkey;

        public enum GhostState {chase, chill}
        

        public Map(string pathMap, string pathIcons)
        {
            ReadMap(pathMap);
            ReadIcons(pathIcons);
            state = State.running;
        }

        public (int, int)[] moves = { (0, -1), (1, 0), (0, 1), (-1, 0) }; //moves for BFS, with prioritites ^>v<
        public int nextx = 5;
        public int nexty = 5;

        public void GhostAI(int from_x, int from_y, int to_x, int to_y)
        {
            Queue<(int, int)> queue = new Queue<(int, int)>();
            int[,] visited = new int[width, height];
            bool reached = false;
            int length = -1;

            visited[from_x, from_y] = 1;
            queue.Enqueue((from_x, from_y));

            while (queue.Count!=0 && !reached)
            {
                (int, int) state = queue.Dequeue();
                reached = CheckLayer(state, queue, visited, to_x, to_y);
            }

            
        }

        public bool CheckLayer ((int,int) state, Queue<(int,int)> queue, int[,] visited, int to_x, int to_y)
        {
            (int, int) new_state;

            int i = state.Item1;
            int j = state.Item2;
            

            for (int k = 0; k < moves.Length; k++)
            {
                int x = i + moves[k].Item1;
                int y = j + moves[k].Item2;
                new_state = (x, y);
                if (visited[x,y] == 0)
                {
                    if (x == to_x && y == to_y)
                    {
                        visited[x, y] = visited[i, j] + 1;
                        nextx = i;
                        nexty = j;
                        return true;
                    }
                    else if (IsEmptyGhost(x, y))
                    {
                        visited[x, y] = visited[i, j] + 1;
                        queue.Enqueue(new_state);
                    }
                    else
                    {
                        visited[x, y] = -100;
                    }

                }
            }
                return false;
        }

        public void ChoseNextCell(Ghost ghost, int x, int y, GhostState state)
        {
            int newx;
            int newy;

            int targx = 0;
            int targy = 0;

            int bestx = x;
            int besty = y;
            int minlength = int.MaxValue;

            switch (state)
            {
                case GhostState.chase:
                    targx = horace.x;
                    targy = horace.y;
                    break;
                case GhostState.chill:
                    targx = door[0];
                    targy = door[1];
                    break;
                default:
                    break;
            }

            

            foreach ((int,int) move in moves)
            {
                newx = x + move.Item1;
                newy = y + move.Item2;

                if(newx!= ghost.prevx || newy != ghost.prevy)
                {
                    if(IsEmptyGhost(newx, newy))
                    {
                        int len = ((newx - targx) * (newx - targx)) + ((newy - targy) * (newy - targy)); //choses the shortest linear distance to the target from the neighbouring cells with a particular prioritite
                        if (minlength > len)
                        {
                            minlength = len;
                            bestx = newx;
                            besty = newy;
                        }
                    }
                }
            }
            nextx = bestx;
            nexty = besty;
            ghost.prevx = x;
            ghost.prevy = y;
            
        }

        public void InserObject(int x, int y, char who)
        {
            plan[x, y] = who;
            plan[x + 1, y] = '-';
            plan[x, y + 1] = '-';
            plan[x + 1, y + 1] = '-';
        }

        public void ReadMap(string path)
        {
            Ghosts = new List<MovingObject>();

            System.IO.StreamReader sr = new System.IO.StreamReader(path);
            width = int.Parse(sr.ReadLine());
            height = int.Parse(sr.ReadLine());
            plan = new char[width, height];
            coinsLeft = 0;
            pill = false;
            int doorind = 0;
            coinsplan = new bool[width, height];

            for (int y = 0; y < height; y++)
            {
                string radek = sr.ReadLine();
                for (int x = 0; x < width; x++)
                {
                    char znak = radek[x];
                    plan[x, y] = znak;

                    // important objects
                    switch (znak)
                    {
                        case 'H':
                            this.horace = new Horace(this, x, y);
                            break;
                        case 'G':
                            Ghost ghost = new Ghost(this, x, y);
                            Ghosts.Add(ghost);
                            break;
                        case 'C': //coin
                            coinsLeft += 1;
                            coinsplan[x, y] = true;
                            break;
                        case 'p':
                            pill = true;
                            break;
                        case '|':
                            door[doorind] = x;
                            door[doorind+1] = y;
                            doorind += 2;
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
            icons = new Bitmap[15];
            int indx = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Rectangle rect = new Rectangle(j * sx / 2, i * sx / 2, sx / 2, sx / 2);
                    icons[indx] = bmp.Clone(rect, System.Drawing.Imaging.PixelFormat.DontCare);
                    indx += 1;
                }
            }
            Rectangle rect1 = new Rectangle(2 * sx, 0, sx, sx); //horace
            icons[8] = bmp.Clone(rect1, System.Drawing.Imaging.PixelFormat.DontCare);

            rect1 = new Rectangle(7 * sx, 0, sx, sx); //ghost
            icons[9] = bmp.Clone(rect1, System.Drawing.Imaging.PixelFormat.DontCare);

            rect1 = new Rectangle(22 * sx / 2, 0, sx / 2, sx / 2);
            icons[10] = bmp.Clone(rect1, System.Drawing.Imaging.PixelFormat.DontCare);
            rect1 = new Rectangle(23 * sx / 2, 0, sx / 2, sx / 2);
            icons[11] = bmp.Clone(rect1, System.Drawing.Imaging.PixelFormat.DontCare);
        }

        public void PrintOut(Graphics g, int sirkaVyrezuPixely, int vyskaVyrezuPixely)
        {
            int sirkaVyrezu = 4 * sirkaVyrezuPixely / sx;
            int vyskaVyrezu = 4 * vyskaVyrezuPixely / sx;

            if (sirkaVyrezu > width)
                sirkaVyrezu = width;

            if (vyskaVyrezu > height)
                vyskaVyrezu = height;

            // urcit LHR vyrezu:
            int dx = horace.x - sirkaVyrezu / 2;
            if (dx < 0)
                dx = 0;
            if (dx + sirkaVyrezu - 1 >= this.width)
                dx = this.width - sirkaVyrezu;

            int dy = horace.y - vyskaVyrezu / 2;
            if (dy < 0)
                dy = 0;
            if (dy + vyskaVyrezu - 1 >= this.height)
                dy = this.height - vyskaVyrezu;

            for (int x = 0; x < sirkaVyrezu; x++)
            {
                for (int y = 0; y < vyskaVyrezu; y++)
                {
                    int mx = dx + x; // index do mapy
                    int my = dy + y; // index do mapy

                    char c = plan[mx, my];
                    if (c != '-')
                    {
                        int indexObrazku = " abcdYXCHG|>".IndexOf(c); // 0..
                        g.DrawImage(icons[indexObrazku], x * sx / 2, y * sx / 2);
                    }
                    if (coinsplan[mx, my] && plan[mx, my] != 'G' && plan[mx, my] != '-') //Saving the coins from ghosts
                    {
                        g.DrawImage(icons[" abcdYXCHG|>".IndexOf('C')], x * sx / 2, y * sx / 2);
                        plan[mx, my] = 'C';
                    }
                    foreach (Ghost ghost in Ghosts)
                    {
                        if (mx == ghost.x && my == ghost.y && !CheckCell(mx, my, 'G'))
                        {
                            g.DrawImage(icons[" abcdYXCHG|>".IndexOf('G')], x * sx / 2, y * sx / 2);
                            InserObject(mx, my, 'G');
                        }
                    }
                }
            }
        }

        public bool IsOpenDoor (int x, int y)
        {
            if (plan[x+1,y] == '>' && plan[x+1, y+1] == '>')
            {
                return true;
            }
            return false;
        }

        public bool IsEmptyHorace(int x, int y)
        {
            if (!" -HC".Contains(plan[x, y]))
                return false;
            if (!" -HC".Contains(plan[x + 1, y]))
                return false;
            if (!" -HC".Contains(plan[x, y + 1]))
                return false;
            if (!" -HC".Contains(plan[x + 1, y + 1]))
                return false;
            return true;

        }

        public bool IsEmptyGhost (int x, int y)
        {
            if (!" -CGH".Contains(plan[x, y]))
                return false;
            if (!" -CGH".Contains(plan[x+1, y]))
                return false;
            if (!" -CGH".Contains(plan[x, y+1]))
                return false;
            if (!" -CGH".Contains(plan[x+1, y+1]))
                return false;
            return true;
           
        }

        public bool IsCoin (int x, int y)
        {
            if (plan[x, y] == 'C')
                return true;
            if (plan[x+1, y] == 'C')
                return true;
            if (plan[x+1, y+1] == 'C')
                return true;
            if (plan[x, y+1] == 'C')
                return true;
            return false;
        }

        public bool IsHorace (int x, int y)
        {
            if (plan[x, y] == 'H')
                return true;
            if (plan[x + 1, y] == 'H')
                return true;
            if (plan[x + 1, y + 1] == 'H')
                return true;
            if (plan[x, y + 1] == 'H')
                return true;
            return false;
        }

        public bool CheckCell (int x, int y, char who)
        {
            if (plan[x, y] == who)
                return true;
            if (plan[x + 1, y] == who)
                return true;
            if (plan[x + 1, y + 1] == who)
                return true;
            if (plan[x, y + 1] == who)
                return true;
            return false;
        }

        public void EatCoin(int x, int y)
        {
            int cx = x;
            int cy = y;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (plan[x+i, y+j] == 'C')
                    {
                        cx = x + i;
                        cy = y + j;
                        break;
                    }
                }
            }

            coinsLeft -= 1;
            plan[cx, cy] = ' ';
            coinsplan[cx, cy] = false;
            if (coinsLeft == 0)
                OpenTheDoor();
        }

        public void OpenTheDoor()
        {
            plan[door[0], door[1]] = '>';
            plan[door[2], door[3]] = '>';
        }

        public void Move (int fromx, int fromy, int tox, int toy)
        {
            char c = plan[fromx, fromy];
            plan[fromx + 1, fromy] = ' ';
            plan[fromx + 1, fromy + 1] = ' ';
            plan[fromx, fromy + 1] = ' ';
            plan[fromx, fromy] = ' ';

            InserObject(tox, toy, c);

            // podivat se, jestli tam nestal hrdina:
            if (c == 'H')
            {
                horace.x = tox;
                horace.y = toy;
                return; // kdyz na [zY,zX] stoji hrdina, tak tam nic jineho nestoji
            }

            // najit pripadny pohyblivyPrvek a zmenit mu polohu :
            foreach (Ghost ghost in Ghosts)
            {
                if ((ghost.x == fromx) && (ghost.y == fromy))
                {
                    ghost.x = tox;
                    ghost.y = toy;
                    break; // jakmile tam stoji jeden, tak uz tam nikdo jiny nestoji
                }
            }
        }

        public void MoveObjects (PressedKey pressedKey)
        {
            this.pressedkey = pressedKey;
            
            foreach (Ghost ghost in Ghosts)
            {
                ghost.MakeMove();
            }

            horace.MakeMove();
        }


    }
/*
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

    }*/




}


