float _SpillRemoval;
float _Tolerance;
float _Threshold;
float _YCgCoMult;
int _Inverse;
int _YCgCo;


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

fixed colorNormRGB2XYZ(fixed val) {
    if(val > 0.04045) {
        return pow((val + 0.055) / 1.055, 2.4);
    }
    return val / 12.92;
}

// http://www.brucelindbloom.com/index.html?Eqn_RGB_XYZ_Matrix.html
// and: http://www.easyrgb.com/en/math.php
fixed3 sRGB2XYZ(fixed3 rgb) {

    rgb.r = colorNormRGB2XYZ(rgb.r);
    rgb.g = colorNormRGB2XYZ(rgb.g);
    rgb.b = colorNormRGB2XYZ(rgb.b);

    fixed3x3 mat = fixed3x3(
        0.4124564, 0.3575761, 0.1804375,
        0.2126729, 0.7151522, 0.0721750,
        0.0193339, 0.1191920, 0.9503041
    );
    return mul(mat, rgb);
}

fixed colorNormXYZ2LAB(fixed val) {
    if(val > 0.008856f) {
        return pow(val, 1.0f / 3.0f);
    }
    return 7.787f * val + 0.137931f;
}

fixed3 XYZ2LAB(fixed3 xyz, fixed3 refCol) {
    xyz.x = xyz.x / refCol.x;
    xyz.y = xyz.y / refCol.y;
    xyz.z = xyz.z / refCol.z;

    xyz.x = colorNormXYZ2LAB(xyz.x);
    xyz.y = colorNormXYZ2LAB(xyz.y);
    xyz.z = colorNormXYZ2LAB(xyz.z);

    return fixed3(
        (116.0f * xyz.y) - 16.0f,
        500.0f * (xyz.x  - xyz.y),
        200.0f * (xyz. y - xyz.z)
    );
}

fixed deltaE_CIE76(fixed3 lab1, fixed3 lab2) {
    return sqrt(
        pow(lab2.x - lab1.x, 2) +
        pow(lab2.y - lab1.y, 2) +
        pow(lab2.z - lab1.z, 2)
    );
}

fixed deltaE_CIE76_sRGB(fixed3 srgb, fixed3 ref) {
    fixed3 refCol = fixed3(.95047f, 1.00f, 1.08883f);
    // convert to LAB
    srgb = XYZ2LAB(sRGB2XYZ(srgb), refCol);
    ref = XYZ2LAB(sRGB2XYZ(ref), refCol);
    return deltaE_CIE76(srgb, ref);    
}

/* 
fixed3 sRGB2CAT02(fixed3 col) {

    fixed3x3 mat = fixed3x3(
        0.39047250, 0.54990437, 0.00890159,
        0.07092586, 0.96310739, 0.00135809,
        0.02314268, 0.12801221, 0.93605194
    );

    return mul(mat, col);
    
}
*/

fixed4 chromaKey(fixed4 col, fixed4 _TargetColor) {

    if(_YCgCo) {
        // return greenFilter(col, _TargetColor);
        // YCGCO Color Space
        fixed3 ycgco = rgb2ycgco(col.rgb);
        fixed2 target = rgb2ycgco(_TargetColor.rgb).yz;
        // Adaptive Threshold over multiple samples - Otsu’s Binarization
        fixed d = distance(ycgco.yz, target) * _YCgCoMult;
        d = smoothstep(_Threshold * 10, (_Threshold + _Tolerance) * 10, d);
        return fixed4(col.rgb, d);
        // Spill Removal Part
        fixed3 ycgco2 = rgb2ycgco(col);
        fixed sub = dot(target, ycgco2.yz) / dot(target, target);
        ycgco2.yz -= target * (sub + 0.5) * _SpillRemoval;
        fixed3 col2 = ycgco2rgb(ycgco2);
        if(_Inverse) {
            d = 1 - d;
        }
        return fixed4(col2, col.a * d);
    }

    
    fixed3 xyz = sRGB2XYZ(fixed3(1, 0.49804, 0.24706));
    // return fixed4(xyz, 1);

    fixed3 lab = XYZ2LAB(fixed3(0.49731f, 0.36804f, 0.09187f), fixed3(0.95047f, 1.00f, 1.08883f));
    // return fixed4(lab / 67.0f, 1);
    fixed d2 = deltaE_CIE76_sRGB(col.xyz, _TargetColor.xyz) / 100;
    // d = d / 120.0f;
    // d = smoothstep(_Threshold, (_Threshold + _Tolerance), d);
    d2 = smoothstep(_Threshold, (_Threshold + _Tolerance), d2);
    // return 1 - (d);
    return fixed4(col.rgb, col.a * d2);
}

fixed3 spillRemoval(fixed3 rgb, fixed3 _TargetColor) {
        fixed2 target = rgb2ycgco(_TargetColor.rgb).yz;
        fixed3 ycgco = rgb2ycgco(rgb);
        fixed sub = dot(target, ycgco.yz) / dot(target, target);
        ycgco.yz -= target * (sub + 0.5) * _SpillRemoval;
        fixed3 col = ycgco2rgb(ycgco);
        return col;
}