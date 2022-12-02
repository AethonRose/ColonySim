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

        public Color water;
        public Color ground;
        public Color grass;
        public Color sand;
        

        public Node[,] grid;
        Node[,] gridClone;

        int maxX;
        int maxY;

        //Conways Game of Life
        public int startGroundRate = 45;
        public Simulation currentStep;
        public Simulation beachStep;

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
                    //Creating each pixel as a node, setting pixel.x & y == pixels location
                    Node pixel = new Node();
                    pixel.x = x;
                    pixel.y = y;

                    //Setting each pixel = pixel
                    grid[x, y] = pixel;

                    //If Random(0-100) < 20 set x,y pixel of texture to ground
                    if (Random.Range(0, 100) < startGroundRate)
                    {
                        pixel.isGround = true;
                        //Setting pixel at x,y position to ground color
                        worldSprite.texture.SetPixel(x, y, ground);
                    }
                    else
                    {
                        pixel.isGround = false;
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
    
        //Calls for generation of each step in Game Of Life Simulation
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
                    Node pixel = grid[x, y];

                    bool alive = true;
                    //Getting NodeState of pixel given n
                    NodeState state = IsAlive(pixel);
                    //If pixel is water sets pixel to dead
                    if (state == NodeState.water)
                        alive = false;

                    //If pixel is alive/land; isGround = true
                    if(alive)
                    {
                        Color targetColor = ground;
                        //If pixels NodeState is grass set targetColor = grass
                        //if (state == NodeState.grass)
                        //    targetColor = grass;

                        pixel.isGround = true;
                        worldSprite.texture.SetPixel(x, y, targetColor);
                    }
                    //If pixel is dead/water; isGround = false
                    else
                    {
                        pixel.isGround = false;
                        worldSprite.texture.SetPixel(x, y, water);
                    }

                }
            }

            BeachStep();

            worldSprite.texture.Apply();
            //Clearing gridClone as next step generated new gridClone
            gridClone = null;
        }
        
        public void BeachStep()
        {
            //Copying grid into cloneGrid
            gridClone = new Node[maxX, maxY];
            System.Array.Copy(grid, gridClone, grid.Length);

            //Loops through width & height or worldSprite
            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    //Creating each pixel as a node, setting pixel.x & y == pixels location
                    Node pixel = grid[x, y];
                    NodeState state = Beach(pixel);

                    if (state == NodeState.sand)
                    {
                        worldSprite.texture.SetPixel(x, y, sand);
                    }
                }
            }
        }

        //Way to call Step multiple times; Given t
        public void StepMultiple(int t)
        {
            for (int x = 0; x < t; x++)
            {
                Step();
            }
        }

        public NodeState IsAlive(Node pixel)
        {
            return currentStep.GetNodeState(pixel, gridClone, maxX, maxY);
        }

        public NodeState Beach(Node pixel)
        {
            return beachStep.GetNodeState(pixel, gridClone, maxX, maxY);
        }

       
    }
}

