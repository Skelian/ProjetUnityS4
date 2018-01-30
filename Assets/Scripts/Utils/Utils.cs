using System.Collections;
using System.Collections.Generic;

public class Utils {

	public static void Swap<T>(ref T a, ref T b) {
		T tmp = a;
		a = b;
		b = tmp;
	}

}
