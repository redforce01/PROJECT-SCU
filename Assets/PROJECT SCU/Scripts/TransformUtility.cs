using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    public class TransformUtility
    {
        public static GameObject FindGameObjectWithTag(GameObject root, string tag)
        {
            return FindFirstChildWithTag(root, tag);
        }

        // Ư�� �±׸� ���� ù ��° �ڽ� GameObject�� ã�� �Լ�
        private static GameObject FindFirstChildWithTag(GameObject parent, string tag)
        {
            // ��� �ڽ� �˻�
            foreach (Transform child in parent.transform)
            {
                // �ڽ��� �±׿� ��ġ�ϴ� ��� �ش� ��ü ��ȯ
                if (child.CompareTag(tag))
                {
                    return child.gameObject;
                }

                // �ڽ��� �ڽĵ� �˻� (��� ȣ��)
                GameObject foundChild = FindFirstChildWithTag(child.gameObject, tag);
                if (foundChild != null)
                {
                    return foundChild;
                }
            }

            // ��ġ�ϴ� �±׸� ���� �ڽ��� ���� ��� null ��ȯ
            return null;
        }
    }
}
