float _SpillRemoval;
float _Tolerance;
float _Threshold;
int _Debug;
int _Inverse;


// super barebones simplistic green screen removal
fixed4 greenFilter(fixed4 pixel, fixed4 targetColor) {

    fixed4 remain = pixel - fixed4(targetColor.rgb, 1);
    if(length(remain) < _Tolerance) {
        return fixed4(0, 0, 0, 0);
    }
    return pixel;
}

// chroma keying

// RGB to YCgCo color space
// http://en.wiki.hancel.net/wiki/YCoCg
fixed3 rgb2ycgco(fixed3 col) {
    return fixed3(
         0.25 * col.r + 0.50 * col.g + 0.25 * col.b,
        -0.25 * col.r + 0.50 * col.g - 0.25 * col.b,
         0.50 * col.r - 0.50 * col.b 
    );
}

// YCgCo to RGB Color Space (inverse)
fixed3 ycgco2rgb(fixed3 col) {
    return fixed3(
        col.x - col.y + col.z,
        col.x + col.y,
        col.x - col.y - col.z
    );
}

fixed4 chromaKey(fixed4 col, fixed4 _TargetColor) {
    // Chroma Keying
    // LAB Color Space
    fixed3 ycgco = rgb2ycgco(col.rgb);
    fixed2 target = rgb2ycgco(_TargetColor.rgb).yz;
    // Adaptive Threshold over multiple samples - Otsu’s Binarization
    fixed d = distance(ycgco.yz, target) * 100;
    d = smoothstep(_Threshold * 10, (_Threshold + _Tolerance) * 10, d);

    // Spill Removal Part
    fixed3 ycgco2 = rgb2ycgco(col);
    fixed sub = dot(target, ycgco2.yz) / dot(target, target);
    ycgco2.yz -= target * (sub + 0.5) * _SpillRemoval;
    fixed3 col2 = ycgco2rgb(ycgco2);
    if(_Inverse) {
        d = 1 - d;
    }
    if(_Debug) {
        return d;
    }
    return fixed4(col2, d);
}


