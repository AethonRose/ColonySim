using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGen.Simulation
{
    public class SimulationMaster : MonoBehaviour
    {
        //References
        public SpriteRenderer spriteRenderer;
        public Sprite worldBaseSprite;
        Sprite worldSprite;

        public Color ground;
        public Color water;

        public Node[,] grid;

        int maxX;
        int maxY;

        void Start() 
        {
            CreateBase();
        }

        void CreateBase()
        {
            //Instantiating worldSprite into newSprite
            worldSprite = Instantiate(worldBaseSprite);

            maxX = worldSprite.texture.width;
            maxY = worldSprite.texture.height;

            grid = new Node[maxX, maxY];

            //Loops through width & height or newSprite
            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    Node n = new Node();
                    n.x = x;
                    n.y = y;

                    grid[x, y] = n;

                    //If Random(0-100) < 20 set x,y pixel of texture to ground
                    if (Random.Range(0, 100) < 20)
                    {
                        n.isGround = true;
                        worldSprite.texture.SetPixel(x, y, ground);
                    }
                    else
                    {
                        n.isGround = false;
                        worldSprite.texture.SetPixel(x, y, water);
                    }
                }
            }

            //Applying texture changes
            worldSprite.texture.Apply();

            spriteRenderer.sprite = worldSprite;
        }
    
        public void Step()
        {
            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    Node n = grid[x, y];

                    bool isAlive = IsAlive(n);
                    if(isAlive)
                    {
                        n.isGround = true;
                        worldSprite.texture.SetPixel(x, y, ground);
                    }
                    else
                    {
                        n.isGround = false;
                        worldSprite.texture.SetPixel(x, y, water);
                    }

                }
            }

            worldSprite.texture.Apply();
        }

        //Way to call Step multiple times; Given t
        public void StepMultiple(int t)
        {
            for (int x = 0; x < t; x++)
            {
                Step();
            }
        }

        public bool IsAlive(Node n)
        {
            bool result = false;
            int count = 0;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int targetX = x + n.x;
                    int targetY = y + n.y;

                    if(targetX == n.x && targetY == n.y)
                        continue;

                    Node neighborNode = GetNode(targetX, targetY);
                    if (neighborNode != null)
                    {
                        if(neighborNode.isGround)
                        {
                            count++;
                        }
                    }
                }
            }

            if (n.isGround)
            {
                if (count < 2)
                {
                    result = false;
                }

                if (count >= 2)
                {
                    result = true;
                }

                if (count > 3)
                {
                    result = false;
                }
            }
            else
            {
                if (count == 3)
                {
                    result = true;
                }
            }

            return result;   
        }

        Node GetNode(int x, int y)
        {
            if (x < 0 || y < 0 || x > maxX - 1 || y > maxY - 1)
            {
                return null;
            }

            return grid[x, y];
        }
    }
}

