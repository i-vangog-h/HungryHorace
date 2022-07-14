/// GAME ENGINE ///


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

        //for ghosts
        public int nextx;
        public int nexty;
        public int prevx;
        public int prevy;
        public Map.GhostState state;

        public abstract void MakeMove();

    }

    enum PressedKey { none, left, right, up, down }
    enum State { notstarted, running, eaten, win, lost }

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
            else if (map.IsGhost(newx, newy))
            {
                bool ghostfear = false;
                foreach (Ghost ghost in map.Ghosts)
                {
                    if (ghost.state == Map.GhostState.fear)
                    {
                        ghostfear = true;
                        break;
                    }
                }
                if (!ghostfear)
                {
                    map.state = State.eaten;
                }
                else
                {
                    map.KillGhost(newx, newy);
                }
            }
            else if (map.CheckCell(newx, newy, 'p'))
            {
                map.EatPill(newx, newy);
                map.Move(x, y, newx, newy);
            }
            
        }
    }

    class Ghost : MovingObject
    {
        public Ghost(Map gmap, int gx, int gy)
        {
            this.map = gmap;
            this.x = gx;
            this.y = gy;
            this.prevx = gx;
            this.prevy = gy;
            this.state = Map.GhostState.chill;
            
        }
        public override void MakeMove()
        {
            map.ChoseNextCellGhost(this, x, y, state);
            if (map.IsHorace(nextx, nexty))
            {
                if (state != Map.GhostState.fear)
                {
                    map.Move(x, y, nextx, nexty);
                    map.state = State.eaten;
                }
                else
                {
                    map.KillGhost(x, y);
                }
                
            }
            else if (map.IsEmptyGhost(nextx, nexty))
            {
                map.Move(x, y, nextx, nexty);
            }
            
        }
    }

    class Map
    {
        public int score;
        public int level;
        private char[,] plan;
        int width;
        int height;
        public int coinsLeft;
        public bool pill;
        private int[] door = new int[4]; // door's location
        public char[,] helpplan; //a plan with coins' and pill's positions to help keeping track of them

        public State state = State.notstarted;

        Bitmap[] icons;
        int sx; // height of the biggest image (2x2 cells)

        public Horace horace;
        public List<MovingObject> Ghosts;
        public List<int> ghostsplan;

        public PressedKey pressedkey;
        public enum GhostState {chase, chill, fear}

        public (int, int)[] moves = { (0, -1), (1, 0), (0, 1), (-1, 0) }; //moves for ghost's algorithm, with prioritites ^>v<

        //Init

        public Map(string pathMap, string pathIcons, int level)
        {
            ReadMap(pathMap, level);
            ReadIcons(pathIcons);
            state = State.running;
            this.score = 0;
            
        }


        // Methods //

        public void ChoseNextCellGhost(Ghost ghost, int x, int y, GhostState state)
        {
            Random rnd = new Random();

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
                    switch (level)
                    {
                        case 1:                // Different targets for ghosts on different levels
                            targx = door[0];
                            targy = door[1];
                            break;
                        case 2:
                        case 3:
                            targx = width / 2;
                            targy = height / 2;
                            break;
                        default:
                            break;
                    }
                    break;
                case GhostState.fear:
                    targx = rnd.Next(0, width - 1);
                    targy = rnd.Next(0, height - 1);
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
            ghost.nextx = bestx;
            ghost.nexty = besty;
            ghost.prevx = x;
            ghost.prevy = y;
            
        } //ghost algorithm 
        public void ReadMap(string path, int level)
        {
            this.level = level;
            Ghosts = new List<MovingObject>();
            ghostsplan = new List<int>();
            System.IO.StreamReader sr = new System.IO.StreamReader(path);
            string row = sr.ReadLine();
            while(row != $"#{level}")

            {
                row = sr.ReadLine();
            }
            width = int.Parse(sr.ReadLine());
            height = int.Parse(sr.ReadLine());
            plan = new char[width, height];
            coinsLeft = 0;
            pill = false;
            int doorind = 0;
            helpplan = new char[width, height];
            

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
                            ghostsplan.Add(x);
                            ghostsplan.Add(y);
                            break;
                        case 'C': //coin
                            coinsLeft += 1;
                            helpplan[x, y] = 'C';
                            break;
                        case 'p':
                            pill = true;
                            helpplan[x, y] = 'p';
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
            icons = new Bitmap[14];
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
            rect1 = new Rectangle(6 * sx, 0, sx, sx); //frightened ghost
            icons[13] = bmp.Clone(rect1, System.Drawing.Imaging.PixelFormat.DontCare);

            rect1 = new Rectangle(22 * sx / 2, 0, sx / 2, sx / 2); //doors
            icons[10] = bmp.Clone(rect1, System.Drawing.Imaging.PixelFormat.DontCare);
            rect1 = new Rectangle(23 * sx / 2, 0, sx / 2, sx / 2);
            icons[11] = bmp.Clone(rect1, System.Drawing.Imaging.PixelFormat.DontCare);

            rect1 = new Rectangle(3 * sx, 0, sx / 2, sx / 2); //red pill
            icons[12] = bmp.Clone(rect1, System.Drawing.Imaging.PixelFormat.DontCare);

            
        }
        public void PrintOut(Graphics g, int sirkaVyrezuPixely, int vyskaVyrezuPixely)
        {
            int sirkaVyrezu = 4*sirkaVyrezuPixely / sx;
            int vyskaVyrezu = 4*vyskaVyrezuPixely / sx;

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

            bool ghostfear = false;

            foreach (Ghost ghost in Ghosts)
            {
                if (ghost.state == GhostState.fear)
                {
                    ghostfear = true;
                    break;
                }

            }

            for (int x = 0; x < sirkaVyrezu; x++)
            {
                for (int y = 0; y < vyskaVyrezu; y++)
                {
                    int mx = dx + x; // index do mapy
                    int my = dy + y; // index do mapy


                    char c = plan[mx, my];

                    if (c != 'h' && c!='g')   //Don't need to print this out
                    {
                        int indexObrazku = " abcdYXCHG|>pF".IndexOf(c); // 0..
                        g.DrawImage(icons[indexObrazku], x * sx / 2, y * sx / 2);
                    }

                    if (c == 'G' && ghostfear)  //Print out ghost fear icon (ghost's char in plan is still G)
                    {
                        int indexObrazku = " abcdYXCHG|>pF".IndexOf('F'); // 0..
                        g.DrawImage(icons[indexObrazku], x * sx / 2, y * sx / 2);
                    }


                    if (helpplan[mx, my] == 'C' && plan[mx, my] != 'G' && plan[mx, my] != 'g') //Saving the coins and the pill from ghosts (while intersecting)
                    {
                        g.DrawImage(icons[" abcdYXCHG|>pF".IndexOf('C')], x * sx / 2, y * sx / 2);
                        plan[mx, my] = 'C';
                    }
                    else if (helpplan[mx, my] == 'p' && plan[mx, my] != 'G' && plan[mx, my] != 'g')
                    {
                        g.DrawImage(icons[" abcdYXCHG|>pF".IndexOf('p')], x * sx / 2, y * sx / 2);
                        plan[mx, my] = 'p';
                    }


                    foreach (Ghost ghost in Ghosts)  //Ghosts intersection 
                    {
                        if (!ghostfear)
                        {
                            if (mx == ghost.x && my == ghost.y && !CheckCell(mx, my, 'G'))
                            {
                                g.DrawImage(icons[" abcdYXCHG|>pF".IndexOf('G')], x * sx / 2, y * sx / 2);
                                InserObject(mx, my, 'G');
                            }
                        }
                        else
                        {
                            if (mx == ghost.x && my == ghost.y && !CheckCell(mx, my, 'G'))
                            {
                                g.DrawImage(icons[" abcdYXCHG|>pF".IndexOf('F')], x * sx / 2, y * sx / 2);
                                InserObject(mx, my, 'G');
                            }
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
        public void OpenTheDoor()
        {
            plan[door[0], door[1]] = '>';
            plan[door[2], door[3]] = '>';
        }

        public bool IsEmptyHorace(int x, int y)  //Checks empty cell for Horace
        {
            if (!" hHC".Contains(plan[x, y]))
                return false;
            if (!" hHC".Contains(plan[x + 1, y]))
                return false;
            if (!" hHC".Contains(plan[x, y + 1]))
                return false;
            if (!" hHC".Contains(plan[x + 1, y + 1]))
                return false;
            return true;

        }

        public bool IsEmptyGhost (int x, int y)  //Checks empty cell for a ghost
        {
            if (!" gCGpHh".Contains(plan[x, y]))
                return false;
            if (!" gCGpHh".Contains(plan[x+1, y]))
                return false;
            if (!" gCGpHh".Contains(plan[x, y+1]))
                return false;
            if (!" gCGpHh".Contains(plan[x+1, y+1]))
                return false;
            return true;
           
        }

        public bool CheckCell (int x, int y, char who)  //Checks if a square (2x2) contains given object
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

        public bool IsHorace (int x, int y)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (plan[x+i, y+j] == 'h' || plan[x+i, y+j] == 'H')
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }

        public bool IsGhost(int x, int y)

        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (plan[x + i, y + j] == 'g' || plan[x + i, y + j] == 'G')
                    {
                        return true;
                    }
                }
            }

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
            score += 4;
            plan[cx, cy] = ' ';
            helpplan[cx, cy] = ' ';
            if (coinsLeft == 0)
                OpenTheDoor();
        }

        public void EatPill(int x, int y)
        {
            int px = x;
            int py = y;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (plan[x + i, y + j] == 'p')
                    {
                        px = x + i;
                        py = y + j;
                        break;
                    }
                }
            }

            helpplan[px, py] = ' ';
            if (plan[px, py] == 'p') 
            {
                plan[px, py] = ' ';
            }
            score += 15;
            foreach(Ghost ghost in Ghosts)
            {
                ghost.state = GhostState.fear;
            }
        }

        public void KillGhost(int x, int y)
        {
            int gx = x;
            int gy = y;
            bool stop = false;

            for (int i = 0; i < 2; i++) //Finds the Ghost's "square"
            {
                for (int j = 0; j < 2; j++)
                {
                    if (plan[x + i, y + j] == 'G' || plan[x + i, y + j] == 'g')
                    {
                        gx = x + i;
                        gy = y + j;
                        stop = true;
                        break;
                    }

                }
                if (stop)
                {
                    break;
                }
            }

            if (plan[gx, gy] != 'G')   //Find the main char of the Ghost to remove it
            {
                if (plan[gx - 1, gy] == 'G')    //All posible positions of little 'g' relating to the big G
                {
                    gx = gx - 1;
                }
                else if (plan[gx, gy - 1] == 'G')
                {
                    gy = gy - 1;
                }
                else if (plan[gx - 1, gy - 1] == 'G')
                {
                    gx = gx - 1;
                    gy = gy - 1;
                }
            }

            for (int i = 0; i < Ghosts.Count; i++)
            {
                if ((Ghosts[i].x == gx) && (Ghosts[i].y == gy))
                {

                    Ghosts.RemoveAt(i); // Removes from the list of objects


                    for (int k = 0; k < 2; k++) // Removes from plan 
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            if ("Gg".Contains(plan[gx + k, gy + j]))
                            {
                                plan[gx + k, gy + j] = ' ';
                            }
                        }
                    }
                    break;
                }
            }

            score += 30;
        }
        public void RestoreGhosts()
        {
            int max = ghostsplan.Count - Ghosts.Count * 2;
            for (int i = 0; i < max; i+=2)
            {
                int x = ghostsplan[i];
                int y = ghostsplan[i+1];
                InserObject(x, y, 'G');
                Ghost ghost = new Ghost(this, x, y);
                Ghosts.Add(ghost);
            }
        }

        public void InserObject(int x, int y, char who) //Inserts a 2x2 object to a plan with corresponding chars
        {
            plan[x, y] = who;
            char ch = '-';
            switch (who)
            {
                case 'H':
                    ch = 'h';
                    break;
                case 'G':
                    ch = 'g';
                    break;
                default:
                    break;
            }
            if (ch != '-')
            {
                plan[x + 1, y] = ch;
                plan[x, y + 1] = ch;
                plan[x + 1, y + 1] = ch;
            }

        }

        public void Move (int fromx, int fromy, int tox, int toy)
        {
            char c = plan[fromx, fromy];

            plan[fromx + 1, fromy] = ' ';
            plan[fromx + 1, fromy + 1] = ' ';
            plan[fromx, fromy + 1] = ' ';
            plan[fromx, fromy] = ' ';

            InserObject(tox, toy, c);

            // Checks if there was Horace and changes its coordinates
            if (c == 'H')
            {
                horace.x = tox;
                horace.y = toy;
                return; // if yes, then no one else was on [fromx,fromy]
            }

            foreach (Ghost ghost in Ghosts) //if no, then changes coordinates of a ghost, that moved
            {
                if ((ghost.x == fromx) && (ghost.y == fromy))
                {
                    ghost.x = tox;
                    ghost.y = toy;
                    break; 
                }
            }
        }

        public void MoveObjects (PressedKey pressedKey)
        {
            this.pressedkey = pressedKey;
            try
            {
                foreach (Ghost ghost in Ghosts)
                {
                    ghost.MakeMove();
                }
            }
            catch
            {

            }
            horace.MakeMove();

        }

        

    }

}


