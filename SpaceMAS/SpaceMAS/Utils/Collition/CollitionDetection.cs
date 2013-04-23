using System;
using Microsoft.Xna.Framework;
using SpaceMAS.Models;

namespace SpaceMAS.Utils.Collition {
    public class CollitionDetection {

        public static bool IntersectPixels(
            Matrix transformA, int widthA, int heightA, Color[] dataA,
            Matrix transformB, int widthB, int heightB, Color[] dataB, GameObject gameObject1, GameObject gameObject2) {

            //// Hent senter av teksturen, transformer dette punktet til skjermkoordinater
            //Vector2 tempVec = new Vector2(widthA / 2.0f, heightA / 2.0f);
            //Vector2 aOrigin = Vector2.Transform(tempVec, transformA);

            //// Gjør det samme med B
            //tempVec.X = widthB / 2.0f;
            //tempVec.Y = heightB / 2.0f;
            //Vector2 bOrigin = Vector2.Transform(tempVec, transformB);

            //// Generer en max radius for teksturene (hypotenus / 2)
            //double aRadius = Math.Sqrt((widthA * widthA) + (heightA * heightA)) / 2.0f;
            //double bRadius = Math.Sqrt((widthB * widthB) + (heightB * heightB)) / 2.0f;

            //// Finn avstanden mellom punktene
            ////float distance = Vector2.Distance(aOrigin, bOrigin);

            //// Om funksjonen ovenfor ikke fungerer som forventet, er dette hvordan det gjøres ellers: 
            //// d = sqrt(dx^2 + dy^2) 
            //double distance2 = Math.Sqrt(((aOrigin.X - bOrigin.X) * (aOrigin.X - bOrigin.X) +
            //                              (aOrigin.Y - bOrigin.Y) * (aOrigin.Y - bOrigin.Y)));

            //Console.WriteLine(distance2);


            //if (distance2 > (aRadius + bRadius)) {
            //    //Console.WriteLine("Distance was greater, avoided!");
            //    return false;
            //}

            //Console.WriteLine(DateTime.Now.Millisecond + " NOT GREATER! NOT AVOIDED!");
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:

            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++) {

                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++) {

                    // Round to the nearest pixel
                    int xB = (int) Math.Round(posInB.X);
                    int yB = (int) Math.Round(posInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB && 0 <= yB && yB < heightB) {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0) {
                            // then an intersection has been found
                            return true;

                        }
                    }
                    // Move to the next pixel in the row
                    posInB += stepX;
                }
                // Move to the next row
                yPosInB += stepY;
            }
            // No intersection found
            return false;
        }

        //public static bool CheckPerPixelCollision(GameObject a, GameObject b) {

        //    double start = DateTime.Now.Ticks;

        //    /*
        //     * We need to make the transform matrix that goes from A's transform
        //     * to B's transform, because they most likely have different effects
        //     * like scale, rotation, position, and more.
        //     */
        //    Matrix atob = a.Transform * Matrix.Invert(b.Transform);

        //    /*
        //     * Our main loop checks every single column and row of A's texture.
        //     * We need to duplicate this for B.  However, since B has a different 
        //     * transform, we need to transform a normal from A to B in order
        //     * to do the proper checking in B's world
        //     */
        //    Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, atob);
        //    Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, atob);

        //    /*
        //     * 0x0 in A will be different in B since we have different
        //     * transform matrices.  We need to calculate the correct one
        //     * for B's start by using the atob transform we created.
        //     */
        //    Vector2 iBPos = Vector2.Transform(Vector2.Zero, atob);

        //    //Start from 0x0 on A, and the similar coordinates in B
        //    for (int deltax = 0; deltax < a.Texture.Width; deltax++)
        //    {
        //        Vector2 bpos = iBPos;
        //        for (int deltay = 0; deltay < a.Texture.Height; deltay++)
        //        {
        //            int bx = (int)bpos.X;
        //            int by = (int)bpos.Y;

        //            /*
        //             * The values need to be within the texture dimensions.
        //             * Otherwise, the program will throw an ArrayIndexOutOfBounds
        //             * exception
        //             */
        //            if (bx >= 0 && bx < b.Texture.Width && by >= 0 &&
        //                by < b.Texture.Height)
        //            {
        //                //CHANGE THE '> 150' TO '!= 0' WHEN YOU USE THIS CODE
        //                //IF YOU HAVE FULLY TRANSPARENT PIXELS!
        //                if (a.TextureData[deltax + deltay * a.Texture.Width].A != 0
        //                    && b.TextureData[bx + by * b.Texture.Width].A != 0) {
        //                    return true;
        //                }
        //            }

        //            /*
        //             * We are looping through every single pixel in column deltax
        //             * for the A texture.  We need to increment the same thing 
        //             * for B's texture.
        //             */
        //            bpos += stepY;
        //        }
        //        iBPos += stepX;
        //    }

        //    Console.WriteLine("FALSE: " + (DateTime.Now.Ticks - start));

        //    return false;
        //}
    }

}