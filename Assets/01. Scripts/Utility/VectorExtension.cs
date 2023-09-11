using UnityEngine;

public static class VectorExtension
{
    public const float EPSILON = 0.01f;

    public static float CCW(Vector3 a, Vector3 b) => Vector3.Cross(a, b).z;
    public static float CCW(this Vector3 point, Vector3 a, Vector3 b) => Vector3.Cross(a - point, b - point).z;

    public static bool IsIntersect(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        // (선분 AB 입장) 점 a를 기준으로 b -> c 외적 값이 반시계방향
        // (선분 AB 입장) 점 a를 기준으로 b -> d 외적 값이 시계방향
        // (선분 CD 입장) 점 c를 기준으로 d -> a 외적 값이 반시계방향
        // (선분 CD 입장) 점 c를 기준으로 d -> b 외적 값이 시계방향
        // 해당 조건이 모두 만족해야 교차한다.
        // 이해 안 되면 그려보셈

        float segmentAB = a.CCW(b, c) * a.CCW(b, d);
        float segmentCD = c.CCW(d, a) * c.CCW(d, b);

        // 두 직선이 동일선상에 있거나 끝 점이 겹치는 경우
        if(segmentAB == 0 && segmentCD == 0)
        {
            if (b == c && d == a)
                return false; // 동일한 직선일 경우 교차하지 않음

            if (b == c) // 선분 A의 종점과 선분 B의 시점이 같을 경우 교점 존재
                return true;

            if (d == a) // 선분 B의 종점과 선분 A의 시점이 같을 경우 교점 존재
                return true;
        }

        // 선분 AB 입장의 조건이 일치하다면 음수 또는 0이어야 함
        // 선분 CD 입장의 조건이 일치하다면 음수 또는 0이어야 함
        return segmentAB <= 0 && segmentCD <= 0;
    }

    /// <summary>
    /// 두 선분의 교점을 찾는 함수
    /// </summary>
    /// <returns>두 선분의 교점이 찾아졌으면 true 아니면 false 반환</returns>
    public static bool GetSegmentIntersection(Vector3 a, Vector3 b, Vector3 c, Vector3 d, out Vector3 intersection)
    {
        // 두 선분 평행한 경우 => 한 점에서 닿았을 때만 교차한다 O
        if(GetLineIntersection(a, b, c, d, out intersection) == false)
            return GetParallelTangent(a, b, c, d, out intersection); // 한 점에 닿았을 경우 교점을 구하는 함수

        // 직선의 교점이 선분 A와 선분 B 위에 있으면 true 반환
        return (intersection.InnerSegment(a, b) && intersection.InnerSegment(c, d));
    }

    /// <summary>
    /// 두 직선의 교점을 찾는 함수
    /// </summary>
    /// <returns>두 직선이 평행하거나 동일하다면, 즉 교점을 찾을 수 없다면 false 아니면 true 반환</returns>
    public static bool GetLineIntersection(Vector3 a, Vector3 b, Vector3 c, Vector3 d, out Vector3 intersection)
    {
        Vector3 from = Vector3.Cross(a, b);
        Vector3 to = Vector3.Cross(c, d);

        Vector3 cross = Vector3.Cross(from, to);

        if (Mathf.Abs(cross.z) < EPSILON)
        {
            intersection = Vector3.zero;
            return false; // 평행 또는 동일할 때는 false 반환
        }

        // cross = (x, y, w)
        // 교점 = (x / w, y / w)
        intersection = new Vector3(cross.x / cross.z, cross.y / cross.z);

        return true; // 교점이 찾아지면 true 반환
    }

    /// <summary>
    /// 평행한 두 선분의 교점 => 두 선분이 동일선상에 있으며 한 점에서 닿은 경우의 교점을 구하는 함수
    /// </summary>
    /// <returns>두 선분이 동일선상에 있으며 한 점에서 닿은 경우 true 아니면 false 반환</returns>
    private static bool GetParallelTangent(Vector3 a, Vector3 b, Vector3 c, Vector3 d, out Vector3 intersection)
    {
        intersection = Vector3.zero;

        // a를 시점으로 b를 종점으로 고정
        if(b.x < a.x && b.y < a.y)
        {
            Vector3 temp = b;
            b = a;
            a = temp;
        }

        // c를 시점으로 d를 종점으로 고정
        if(d.x < c.x && d.y < c.y)
        {
            Vector3 temp = d;
            d = c;
            c = temp;
        }

        // 점 c를 기준으로 a와 b를 외적했을 때 값이 0이 아니면 평행하지 않다
        // 그렇게 됐을 때 선분 A와 선분 B는 동일선상에 있지 않다
        if(Vector3.Cross(a - c, b - c).z != 0) 
            return false; // 선분 A와 선분 B는 동일선상에 있지 않을 때 교점을 찾을 수 없기 때문에 false 반환

        // 동일선상에 있지만 모든 꼭짓점들이 같은 경우 동일한 직선이다
        if(b == c && d == a) 
            return false; // 동일한 직선일 경우 교점을 찾을 수 없기 때문에 false 반환

        if(b == c) // 선분 A의 종점과 선분 B의 시점이 같을 경우 교점은 b(선분 A의 종점)와 c(선분 B의 시점)
            intersection = b;

        if(d == a) // 선분 B의 종점과 선분 A의 시점이 같을 경우 교점은 d(선분 B의 종점)와 a(선분 A의 시점)
            intersection = d;

        return true; // 교점이 찾아졌을 때 true 반환
    }

    /// <summary>
    /// 한 점이 선분 a-b 위에 위치하는지를 확인하는 함수
    /// </summary>
    /// <returns>한 점이 선분 a-b 위에 위치한다면 true 아니면 false 반환</returns>
    public static bool InnerSegment(this Vector3 point, Vector3 a, Vector3 b)
    {
        Vector3 v1 = point - a; // a -> point 벡터
        Vector3 v2 = b - a; // a -> b 벡터

        Debug.Log(Mathf.Abs(v1.y / v1.x - v2.y / v2.x));

        if((v1.y / v1.x - v2.y / v2.x) < EPSILON) // v1의 기울기와 v2의 기울기의 오차가 0.01보다 작고
        {
            Vector3 min = Vector3.Min(a, b); // 바운딩 박스의 최소값
            Vector3 max = Vector3.Max(a, b); // 바운딩 박스의 최대값

            // 바운딩 박스 안에 있다면
            if((min.x <= point.x && point.x <= max.x) && (min.y <= point.y && point.y <= max.y))
                return true; // true 반환
        }

        return false; // 점이 선분 밖에 위치한다면 false 반환
    }
}
