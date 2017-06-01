float _OpenGL;

// OpenGL Depth Buffer ranges from [-1...1], this maps linearly
// based on the projection parameters of that camera
fixed LinearGLDepth(fixed n, fixed f, fixed z) {
    // 2 * near / (far + near - z * (far - near)) - logarithmic depth in OpenGL
    return (2 * n) / (f + n - z * (f - n));
}

// D3D is already between 0..1
// returns the depth mapped linearly between 0..1
fixed LinearDepth(fixed4 projection, fixed z) {
    if(projection.x == -1) { // x=1 -> D3D; x=-1 -> OpenGL
        return LinearGLDepth(projection.y, projection.z, z);
    }
    return z;
}