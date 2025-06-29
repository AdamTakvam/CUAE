package metreos.util;

/**
 * Description of ByteArrayUtils
 */
public class ByteArrayUtils
{
	/**
	 * @param n
	 * @param k
	 * @return a mask of n bytes with the k most significant bits 1.
	 */
	public static byte[] newMask( int n, int k )
	{
		byte[] mask = new byte[n];
		for (int i = 0; i < n; i++)
		{
			if (k >= 8)
			{
				mask[i] = -1;
				k -= 8;
			}
			else if (k == 0)
			{
				mask[i] = 0;
			}
			else // k = 1 to 7
			{
				mask[i] = (byte) (-1 << (8-k));
				k = 0;
			}
		}
		return mask;
	}
	
	/**
	 * @param name
	 * @param mask
	 */
	public static void dumpMask( String name, byte[] mask )
	{
		System.out.print( name );
		System.out.print( '=' );
		for (int i = 0; i < mask.length; i++)
		{
			if (i != 0)
				System.out.print( '.' );
			System.out.print( mask[i] & 255 );
		}
		System.out.println();
	}
	
	/**
	 * @param a
	 * @param b
	 * @return the result of bitwise a & b
	 */
	public static byte[] andMask( byte[] a, byte[] b )
	{
		Assertion.check( a.length == b.length, "a.length == b.length" );
		byte[] c = new byte[a.length];
		for (int i = 0; i < a.length; i++)
			c[i] = (byte) (a[i] & b[i]);
		return c;
	}

	/**
	 * @param a
	 * @param b
	 * @return the result of bitwise a == b
	 */
	public static boolean eqMask( byte[] a, byte[] b )
	{
		Assertion.check( a.length == b.length, "a.length == b.length" );
		for (int i = 0; i < a.length; i++)
			if (a[i] != b[i])
				return false;
		return true;
	}
}
