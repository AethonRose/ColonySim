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
        Node[,] gridClone;

        int maxX;
        int maxY;

        //Conways Game of Life
        public int startGroundRate = 45;
        public Simulation currentStep;

        void Start() 
        {
            CreateBase();
        }

        void CreateBase()
        {
            //Instantiating worldSprite into newSprite
            worldSprite = Instantiate(worldBaseSprite);

            //Setting maxX and maxY to worldSprites width and height
            maxX = worldSprite.texture.width;
            maxY = worldSprite.texture.height;

            //Creating grid to width x height of worldSprite
            grid = new Node[maxX, maxY];

            //Loops through width & height or worldSprite
            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    //Creating each pixel as a node, setting n.x & n.y == pixels location
                    Node n = new Node();
                    n.x = x;
                    n.y = y;

                    //Setting each pixel == n
                    grid[x, y] = n;

                    //If Random(0-100) < 20 set x,y pixel of texture to ground
                    if (Random.Range(0, 100) < startGroundRate)
                    {
                        n.isGround = true;
                        //Setting pixel at x,y position to ground color
                        worldSprite.texture.SetPixel(x, y, ground);
                    }
                    else
                    {
                        n.isGround = false;
                        //Setting pixel at x,y position to water color
                        worldSprite.texture.SetPixel(x, y, water);
                    }
                }
            }

            //Applying texture changes
            worldSprite.texture.Apply();
            //Setting spriteRederer sprite to the worldSprite
            spriteRenderer.sprite = worldSprite;
        }
    
        public void Step()
        {
            //Copying grid into cloneGrid
            gridClone = new Node[maxX, maxY];
            System.Array.Copy(grid, gridClone, grid.Length);

            //Loops through width & height or worldSprite
            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    //Creating each pixel as a node, setting n.x & n.y == pixels location
                    Node n = grid[x, y];

                    //Getting isAlive value by calling IsAlive and giving n pixel
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
            //Clearing gridClone as next step generated new gridClone
            gridClone = null;
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
            return currentStep.isAlive(n, gridClone, maxX, maxY);

              
        }

       
    }
}

