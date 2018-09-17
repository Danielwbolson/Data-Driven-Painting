using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SculptingVis {
	[System.Serializable]
	public class StyleTypeSocket : StyleSocket {

	}


	public class Objectify<T> : Object {

		public static bool operator ==(Objectify<T> A, Objectify<T> B)
		{
			if (object.ReferenceEquals(A, null))
			{
				return object.ReferenceEquals(B, null);
			}

			return A.Equals(B);
		}

		public static bool operator !=(Objectify<T> A, Objectify<T> B)
		{
			if (object.ReferenceEquals(A, null))
			{
				return !object.ReferenceEquals(B, null);
			}

			return !A.Equals(B);
		}
		public override string ToString() {
			return  "" + value ;
		}
		public static implicit operator T(Objectify<T> x)
		{
			return x.value;
		}

		public Objectify(T initial) {
			value = initial;
		}
		public T value;
	}
	public class MinMax<T> : Object
    {
        public MinMax(T lower, T upper)
        {
            lowerBound = lower;
            upperBound = upper;

			lowerValue = lower;
			upperValue = upper;
        }
        public override string ToString()
        {
            return lowerBound + " < " + upperBound;
        }
		public T lowerValue;
		public T upperValue;
        public T lowerBound;
        public T upperBound;
    }
	public class Range<T> :Object {
		public static bool operator ==(Range<T> A, Range<T> B)
		{
			if (object.ReferenceEquals(A, null))
			{
				return object.ReferenceEquals(B, null);
			}

			return A.Equals(B);
		}

		public static bool operator !=(Range<T> A, Range<T> B)
		{
			if (object.ReferenceEquals(A, null))
			{
				return !object.ReferenceEquals(B, null);
			}

			return !A.Equals(B);
		}
		public override string ToString() {
			return lowerBound + " < " + value + " < " + upperBound;
		}
		public static implicit operator T(Range<T> x)
		{
			return x.value;
		}
		public Range(T lower, T upper) {
			lowerBound = lower;
			upperBound = upper;
			value = lower;
		}
		public Range(T lower, T upper, T initial) {
			lowerBound = lower;
			upperBound = upper;
			value = initial;
		}
		public T lowerBound;
		public T upperBound;
		public T value;
	}

		// 	public virtual Object GetInput() {
		// 	return _input;
		// }
		
	public class StyleTypeSocket<T> : StyleTypeSocket {
		public override JSONObject serialize() {
			JSONObject json = new JSONObject();

			if(GetInput() is Range<int>) {
				json.AddField("value",((Range<int>)GetInput()).value);
			}
			if(GetInput() is Range<float>) {
				json.AddField("value",((Range<float>)GetInput()).value);
			}
			if(GetInput() is Range<bool>) {
				json.AddField("value",((Range<bool>)GetInput()).value);
			}
			if(GetInput() is MinMax<float>) {
				json.AddField("lowervalue",((MinMax<float>)GetInput()).lowerValue);
				json.AddField("uppervalue",((MinMax<float>)GetInput()).upperValue);
			}
			if(GetInput() is Objectify<Color>) {
				json.AddField("red",((Objectify<Color>)GetInput()).value.r);
				json.AddField("green",((Objectify<Color>)GetInput()).value.g);
				json.AddField("blue",((Objectify<Color>)GetInput()).value.b);
				json.AddField("alpha",((Objectify<Color>)GetInput()).value.a);
			}
			if(GetInput() is Objectify<float>) {
				json.AddField("value",((Objectify<float>)GetInput()).value);
			}
			return json;
		}
		public override void applySeralization(JSONObject json) {
			if(GetInput() is Range<int>) {
				 json.GetField(out ((Range<int>)GetInput()).value, "value",0);
			}
			if(GetInput() is Range<float>) {
				json.GetField(out ((Range<float>)GetInput()).value, "value",0);
			}
			if(GetInput() is Range<bool>) {
				json.GetField(out ((Range<bool>)GetInput()).value,"value",false);
			}
			if(GetInput() is MinMax<float>) {

				json.GetField(out ((MinMax<float>)GetInput()).lowerValue,"lowervalue",0);
				json.GetField(out ((MinMax<float>)GetInput()).upperValue,"uppervalue",0);

			}
			if(GetInput() is Objectify<Color>) {

				json.GetField(out ((Objectify<Color>)GetInput()).value.r, "red",0);
				json.GetField(out ((Objectify<Color>)GetInput()).value.g, "green",0);
				json.GetField(out ((Objectify<Color>)GetInput()).value.b, "blue",0);
				json.GetField(out ((Objectify<Color>)GetInput()).value.a, "alpha",0);


			}
			if(GetInput() is Objectify<float>) {
				json.AddField("value",((Objectify<float>)GetInput()).value);
				json.GetField(out ((Objectify<float>)GetInput()).value, "value",0);
			}
		}
		public override string GetTypeTag() {
			return (typeof(T) + "_TYPESOCKET").ToUpper();
		}


		public override bool DoesAccept(StyleSocket incoming) {
			return incoming.GetOutput() is T;
		}
	
		// public event Func<T> OnCreateRequest;
		
		// private T CreateInstance()
		// {
		// 	if (OnCreateRequest != null)
		// 		return OnCreateRequest();
		// 	var t = typeof(T);
		// 	if (typeof(Component).IsAssignableFrom(t))
		// 		return (new GameObject(t.Name)).AddComponent<T>();
		// 	return System.Activator.CreateInstance<T>();
		// }

		public StyleTypeSocket<T> Init(string label, StyleModule module) {
			base.Init(label,module,true,false);
		
			return this;
		}

		public virtual Object GetInput() {
			if(base.GetInput() is T)
				return base.GetInput();
			else 
				return null;
		}
	}
}

