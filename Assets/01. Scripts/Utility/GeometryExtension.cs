using UnityEngine;

public static class GeometryExtension
{
    public const float EPSILON = 0.01f;

    public static Bounds GetBoundingBox(this Vector3[] geometry)
    {
        Vector3 min = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
        Vector3 max = new Vector3(-Mathf.Infinity, -Mathf.Infinity, -Mathf.Infinity);

        for(int i = 0; i < geometry.Length; i++)
        {
            Vector3 vertex = geometry[i];
            Mathf.Min(min.x, vertex.x);
            Mathf.Min(min.y, vertex.y);
            Mathf.Max(max.x, vertex.x);
            Mathf.Max(max.y, vertex.y);
        }

        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }

    public static bool InsideGeometry(Vector3 point, Vector3[] geometry)
    {
        int crossed = 0;
        int n = geometry.Length;

        for(int i = 0; i < n; ++i)
        {
            int j = (i + 1) % n;

            // y값이 둘다 크거나 둘다 작으면 해당 면의 바운딩 박스 밖의 점
            if ((geometry[i].y > point.y) != (geometry[j].y > point.y)) 
            {
                // 바운딩 박스의 최소 x값이 현재 point의 x값보다 크다면 point의 반직선이 해당 선분을 교차한 상태
                float boundX = Mathf.Min(geometry[j].x, geometry[i].x);
                if(point.x < boundX)
                    crossed++;
            }
        }
 
        // point에서 그은 x축에 평행한 반직선이 교차한 선분의 개수가 홀수면 내부 아니면 외부
        // 그려보면 이해 됨
        return (crossed % 2 == 1);
    }

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
        a.z = b.z = c.z = d.z = 1;
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

        Debug.DrawLine(intersection, intersection + Vector3.up, Color.black, 1f);
        Debug.DrawLine(intersection, intersection + Vector3.down, Color.black, 1f);
        Debug.DrawLine(intersection, intersection + Vector3.left, Color.black, 1f);
        Debug.DrawLine(intersection, intersection + Vector3.right, Color.black, 1f);

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
        if(c.CCW(a, b) != EPSILON) 
            return false; // 선분 A와 선분 B는 동일선상에 있지 않을 때 교점을 찾을 수 없기 때문에 false 반환

        // 동일선상에 있지만 모든 꼭짓점들이 같은 경우 동일한 직선이다
        if(b == c && d == a) 
            return false; // 동일한 직선일 경우 교점을 찾을 수 없기 때문에 false 반환

        if(b == c) // 선분 A의 종점과 선분 B의 시점이 같을 경우 교점은 b(선분 A의 종점)와 c(선분 B의 시점)
            intersection = b;

        if(d == a) // 선분 B의 종점과 선분 A의 시점이 같을 경우 교점은 d(선분 B의 종점)와 a(선분 A의 시점)
            intersection = d;

        Debug.Log(intersection);

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

        // Debug.Log(Mathf.Abs(v1.y / v1.x - v2.y / v2.x));

        if (v1.normalized == v2.normalized) // 주석 적기
        {
            if(v1.sqrMagnitude <= v2.sqrMagnitude) // 주석 적기
                return true; // true 반환
        }

        return false; // 점이 선분 밖에 위치한다면 false 반환
    }
}
