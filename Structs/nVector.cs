using UnityEngine;

public struct nVector {

	public float x, y, z, w;
	public int length;

	public nVector(float x) {
		this.x = y = z = w = x;
		length = 1;
	}
	public nVector(float x, float y) {
		this.x = x;
		this.y = y;
		this.z = 0;
		this.w = 0;
		length = 2;
	}
	public nVector(float x, float y, float z) {
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = 0;
		length = 3;
	}
	public nVector(float x, float y, float z, float w) {
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
		length = 4;
	}
	public nVector(float[] a) {
		length = a.Length;
		x = y = z = w = 0;
		if (length > 0)
			x = y = z = w = a[0];
		if (length > 1)
			y = a[1];
		if (length > 2)
			z = a[2];
		if (length > 3)
			z = a[3];
	}
	public nVector(nVector a) {
		x = a.x;
		y = a.y;
		z = a.z;
		w = a.w;
		length = a.length;
	}
	public nVector(Vector2 a, float b) {
		x = a.x;
		y = a.y;
		z = b;
		w = 0;
		length = 3;
	}
	public nVector(Vector2 a, float b, float c) {
		x = a.x;
		y = a.y;
		z = b;
		w = c;
		length = 4;
	}
	public nVector(float a, Vector2 b, float c) {
		x = a;
		y = b.x;
		z = b.y;
		w = c;
		length = 4;
	}
	public nVector(float a, float b, Vector2 c) {
		x = a;
		y = b;
		z = c.x;
		w = c.y;
		length = 4;
	}
	public nVector(float a, Vector2 b) {
		x = a;
		y = b.x;
		z = b.y;
		w = 0;
		length = 3;
	}
	public nVector(Vector3 a, float b) {
		x = a.x;
		y = a.y;
		z = a.z;
		w = b;
		length = 4;
	}
	public nVector(float a, Vector3 b) {
		x = a;
		y = b.x;
		z = b.y;
		w = b.z;
		length = 4;
	}

	public float this[int index] {
		get {
			switch (index) {
				case 0: return x;
				case 1: return y;
				case 2: return z;
				case 3: return w;
			}
			throw new System.ArgumentOutOfRangeException();
		}
		set	{
			switch (index) {
				case 0: x = value;
					return;
				case 1: y = value;
					return;
				case 2: z = value;
					return;
				case 3: w = value;
					return;
			}
			throw new System.ArgumentOutOfRangeException();
		}
	}

	public static implicit operator nVector(float a) {
		return new nVector(a);
	}
	public static implicit operator nVector(float[] a) {
		int l = a.Length;
		if (l > 3)
			return new nVector(a[0], a[1], a[2], a[3]);
		if (l > 2)
			return new nVector(a[0], a[1], a[2]);
		if (l > 1)
			return new nVector(a[0], a[1]);
		return new nVector(a[0]);
	}
	public static implicit operator nVector(Vector2 a) {
		return new nVector(a.x, a.y);
	}
	public static implicit operator nVector(Vector3 a) {
		return new nVector(a.x, a.y, a.z);
	}
	public static implicit operator nVector(Vector4 a) {
		return new nVector(a.x, a.y, a.z, a.w);
	}
	public static implicit operator nVector(Color a) {
		return new nVector(a.r, a.g, a.b, a.a);
	}

	public static implicit operator float(nVector a) {
		return a.x;
	}
	public static implicit operator float[](nVector a) {
		float[] b = new float[a.length];
		b[0] = a.x;
		if (a.length > 1)
			b[1] = a.y;
		if (a.length > 2)
			b[2] = a.z;
		if (a.length > 3)
			b[3] = a.w;
		return b;
	}
	public static implicit operator Vector2(nVector a) {
		return new Vector2(a.x, a.y);
	}
	public static implicit operator Vector3(nVector a) {
		return new Vector3(a.x, a.y, a.z);
	}
	public static implicit operator Vector4(nVector a) {
		return new Vector4(a.x, a.y, a.z, a.w);
	}
	public static implicit operator Color(nVector a) {
		return new Color(a.x, a.y, a.z, a.w);
	}

	public static nVector operator +(nVector a, nVector b) {
		a.x += b.x;
		a.y += b.y;
		a.z += b.z;
		a.w += b.w;
		if (b.length > a.length)
			a.length = b.length;
		return a;
	}
}
