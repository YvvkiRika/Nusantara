﻿// <https://github.com/YvvkiRika> wrote this file.
// As long as you retain this notice, you can do whatever you want with this stuff.

using System.Numerics;
using System.Runtime.CompilerServices;

using Silk.NET.Maths;

namespace Nusantara.Maths;

public static partial class MathHelper
{
	#region PI Minus Epsilon

	/// <summary>
	///   Represents the smallest difference value that is less than PI.
	/// </summary>
	/*
	 * Subtracting with float.Epsilon is impossible due to single floating point imprecision.
	 * The bits on MathF.PI is: 1000000010010010000111111011011,
	 * while this constant is:  1000000010010010000111111011010
	 */
	public const float PIMinusEpsilon = MathF.PI - 0.000000119209286f;

	#endregion

	#region Radians Degrees

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float DegreesToRadians(float degrees)
	{
		const float constant = MathF.PI / 180.0f;
		return degrees * constant;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float RadiansToDegree(float radians)
	{
		const float constant = 180.0f / MathF.PI;
		return radians * constant;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double DegreesToRadians(double degrees)
	{
		const double constant = Math.PI / 180.0;
		return degrees * constant;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double RadiansToDegree(double radians)
	{
		const double constant = 180.0 / Math.PI;
		return radians * constant;
	}

	#endregion

	#region Normalize Homogeneous

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3D<T> NormalizeHomogeneous<T>(Vector4D<T> value)
		where T : unmanaged, IFormattable, IEquatable<T>, IComparable<T>
	{
		Vector3D<T> result = Unsafe.As<Vector4D<T>, Vector3D<T>>(ref value);
		result = Vector3D.Divide(result, value.W);

		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 NormalizeHomogeneous(Vector4 value)
	{
		Vector3 result = Unsafe.As<Vector4, Vector3>(ref value);
		result = Vector3.Divide(result, value.W);

		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2D<T> NormalizeHomogeneous<T>(Vector3D<T> value)
		where T : unmanaged, IFormattable, IEquatable<T>, IComparable<T>
	{
		Vector2D<T> result = Unsafe.As<Vector3D<T>, Vector2D<T>>(ref value);
		result = Vector2D.Divide(result, value.Z);

		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 NormalizeHomogeneous(Vector3 value)
	{
		Vector2 result = Unsafe.As<Vector3, Vector2>(ref value);
		result = Vector2.Divide(result, value.Z);

		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T NormalizeHomogeneous<T>(Vector2D<T> value)
		where T : unmanaged, IFormattable, IEquatable<T>, IComparable<T>
	{
		T result = Unsafe.As<Vector2D<T>, T>(ref value);
		result = Scalar.Divide(result, value.Y);

		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float NormalizeHomogeneous(Vector2 value)
	{
		float result = Unsafe.As<Vector2, float>(ref value);
		result /= value.Y;

		return result;
	}

	#endregion

	#region To Euler Angles

	// https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles#Source_code_2
	public static (float yaw, float pitch, float roll) ToEulerAngles(Quaternion q)
	{
		float yaw, pitch, roll;

		// roll (x-axis rotation)
		float sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
		float cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
		roll = MathF.Atan2(sinr_cosp, cosr_cosp);

		// pitch (y-axis rotation)
		float sinp = 2 * (q.W * q.Y - q.Z * q.X);
		if (MathF.Abs(sinp) >= 1)
			pitch = MathF.CopySign(MathF.PI / 2, sinp); // use 90 degrees if out of range
		else
			pitch = MathF.Asin(sinp);

		// yaw (z-axis rotation)
		float siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
		float cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
		yaw = MathF.Atan2(siny_cosp, cosy_cosp);

		return (yaw, pitch, roll);
	}

	#endregion

	#region Create Transformation Matrix

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Matrix4x4 CreateTransformMatrix(Vector3 translation, Quaternion rotation, Vector3 scale)
	{
		// Scaling
		Matrix4x4 matrix = Matrix4x4.CreateScale(scale);
		// Rotating
		matrix = Matrix4x4.Transform(matrix, rotation);
		// Translating
		matrix.Translation = translation;

		return matrix;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Matrix4x4 CreateTransformMatrix(Vector4 translation, Quaternion rotation, Vector4 scale)
	{
		Matrix4x4 matrix = CreateTransformMatrix(
			NormalizeHomogeneous(translation),
			rotation,
			NormalizeHomogeneous(scale));

		return matrix;
	}

	#endregion
}
