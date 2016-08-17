public static class BitArray {

    /*    How To Use:

        One Integer = 32 bits
        Using Bitwise Operators, this script turns integers into arrays of 32 bools, 
        gaining memory over actual bool arrays.
        

            For single Int, use
                myInt = myInt.SetBit(index)
            to set the specified bit to true.

                myInt.SetBit(index)
            won't change the value of the myInt variable


        If you want to manipulate more than 32 bools, use an int array instead.


            For Int Arrays, use 
                myInt[].SetBit(index)
            to set the specified bit to true.

                myInt[].SetBit(48)
            will set the 16th bit of the second integer in myInt[]
            (48 = [32] + 16)


        Visual explanation:

            int myInt = 0
            [00000000 00000000 00000000 00000000]

            creates an array of 32 booleans, all set to 'false'

            myInt = myInt.SetBit(12);
            [00000000 00000000 00010000 00000000]

            the 13th bit has been turned to '1'
            (as with arrays, 1st bit has index 0)

            myInt.GetBit(12) returns 'true'
            myInt returns 4096

            the integer value does not matter as we use it as an array of booleans.


        – 2016 Gwendal Reuzé
    */


    //Int Array

    /// <summary>
    /// Turn the specified bit to the desired value
    /// </summary>
    public static void SetBit(this int []A, int k, bool set) {
        if (set) A[k / 32] |= 1 << (k % 32);
        else A[k / 32] &= ~(1 << (k % 32));
    }

    /// <summary>
    /// Turn the specified bit to 1 (true)
    /// </summary>
    public static void SetBit(this int []A, int k) {
        A[k/32] |= 1 << (k%32);}

    /// <summary>
    /// Turn the specified bit to 0 (false)
    /// </summary>
    public static void ClearBit(this int []A, int k) {
        A[k/32] &= ~(1 << (k%32));}

    /// <summary>
    /// Inverse the state of the specified bit (true / false)
    /// </summary>
    public static void ToggleBit(this int[] A, int k) {
        A[k/32] ^= 1 << (k%32);}

    /// <summary>
    /// Return the current state of the specified bit (true / false)
    /// </summary>
    public static bool GetBit(this int []A, int k) {
        return ((A[k/32] & (1 << (k % 32))) != 0);}

    //Single Int

    /// <summary>
    /// Turn the specified bit to the desired value
    /// </summary>
    public static int SetBit(this int A, int k, bool set) {
        return set ? (A | (1 << k)) : (A & (~(1 << k)));
    }

    /// <summary>
    /// Turn the specified bit to 1 (true)
    /// </summary>
    public static int SetBit(this int A, int k) {
        return A |= 1 << k;}

    /// <summary>
    /// Turn the specified bit to 0 (false)
    /// </summary>
    public static int ClearBit(this int A, int k) {
        return A &= ~(1 << k);}

    /// <summary>
    /// Inverse the state of the specified bit (true / false)
    /// </summary>
    public static int ToggleBit(this int A, int k) {
        return A ^= 1 << k;}

    /// <summary>
    /// Return the current state of the specified bit (true / false)
    /// </summary>
    public static bool GetBit(this int A, int k) {
        return ((A & (1 << k)) != 0);}
}
