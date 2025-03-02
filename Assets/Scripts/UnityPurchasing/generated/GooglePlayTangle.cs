// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("oobCMpDI5nLmh4ZuYNG8RtwbPuV1nyUuWM+eAed9bvB32aVnpMs/Dd4knG9QHCyPJggA3bdJDNbHxEzX3F9RXm7cX1Rc3F9fXtsnTuhWGJlCpzSiRitJiZW3mcOAVjm/5bum1OlcHt72Pi6zQ4bpHRcST0v3gsEoFXdUdvUi1s4sMx+ozOIUZ+bOeBDe3xKKntwEbbpZSQ1JzNbAYIx3w9M+ry6Kp5CF/JGZ0pWz5fa7aDuJulKenXuFosp3GSN2fXhoPJx3ic0KXvYQoRQpH3P+uhewqkczDog7UfQFT7QHo8C8Arwh6HRA9lX0y703btxffG5TWFd02BbYqVNfX19bXl0S8+Z3kw+rsf1hnh+O8CYsG2JCAh2e5R1ZertqYVxdX15f");
        private static int[] order = new int[] { 12,5,9,5,9,5,8,8,9,9,11,13,12,13,14 };
        private static int key = 94;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
