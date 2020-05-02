﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	[SerializeField] private MeshFilter meshFilter;
	[SerializeField] private MeshRenderer meshRenderer;
	[SerializeField] private MeshCollider meshCollider;
	[SerializeField] private Vector2Int position;

	private Mesh mesh;
	private List<Vector3> vertices;
	private List<int> triangles;
	private List<Vector3> normals;
	private List<Vector2> uvs;

	public void Awake()
	{
		mesh = new Mesh();
		meshFilter.sharedMesh = mesh;
		mesh.name = "ChunkMesh";
		mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
		mesh.MarkDynamic();
		meshCollider.sharedMesh = mesh;
		vertices = new List<Vector3>();
		normals = new List<Vector3>();
		uvs = new List<Vector2>();
		triangles = new List<int>();
	}

	public void Initialize(Vector2Int position)
	{
		this.position = position;
	}

	public void Build(ChunkDataManager chunkDataManager)
	{
		Vector2Int renderPosition = 16 * position;
		transform.position = new Vector3(renderPosition.x, 0, renderPosition.y);
		mesh.Clear();
		
		for (int z = 0; z < 16; ++z)
		{
			for (int y = 0; y < 256; ++y)
			{
				for (int x = 0; x < 16; ++x)
				{
					char c = chunkDataManager.GetBlock(position, x, y, z);
					if (c != (char)0)
					{

						if (chunkDataManager.GetBlock(position, x + 1, y, z) == (char)0)
						{
							AddFace(
								new Vector3(x + 1, y, z),
								new Vector3(x + 1, y + 1, z),
								new Vector3(x + 1, y + 1, z + 1),
								new Vector3(x + 1, y, z + 1),
								Vector3.right
							);
							AddTextureFace(chunkDataManager.textureMapper.map[c].right);
						}
						if (chunkDataManager.GetBlock(position, x - 1, y, z) == (char)0)
						{
							AddFace(
								new Vector3(x, y, z + 1),
								new Vector3(x, y + 1, z + 1),
								new Vector3(x, y + 1, z),
								new Vector3(x, y, z),
								-Vector3.right
							);
							AddTextureFace(chunkDataManager.textureMapper.map[c].left);

						}

						if (chunkDataManager.GetBlock(position, x, y + 1, z) == (char)0)
						{
							AddFace(
								new Vector3(x, y + 1, z),
								new Vector3(x, y + 1, z + 1),
								new Vector3(x + 1, y + 1, z + 1),
								new Vector3(x + 1, y + 1, z),
								Vector3.up
							);
							AddTextureFace(chunkDataManager.textureMapper.map[c].top);

						}
						if (chunkDataManager.GetBlock(position, x, y - 1, z) == (char)0)
						{
							AddFace(
								new Vector3(x, y, z),
								new Vector3(x+1, y, z),
								new Vector3(x+1, y, z+1),
								new Vector3(x, y, z+1),
								-Vector3.up
							);
							AddTextureFace(chunkDataManager.textureMapper.map[c].bottom);

						}

						if (chunkDataManager.GetBlock(position, x, y, z + 1) == (char)0)
						{
							AddFace(
								new Vector3(x + 1, y, z + 1),
								new Vector3(x + 1, y + 1, z + 1),
								new Vector3(x, y + 1, z + 1),
								new Vector3(x, y, z + 1),
								Vector3.forward
							);
							AddTextureFace(chunkDataManager.textureMapper.map[c].front);

						}
						if (chunkDataManager.GetBlock(position, x, y, z - 1) == (char)0)
						{
							AddFace(
								new Vector3(x, y, z),
								new Vector3(x, y + 1, z),
								new Vector3(x + 1, y + 1, z),
								new Vector3(x + 1, y, z),
								-Vector3.forward
							);
							AddTextureFace(chunkDataManager.textureMapper.map[c].back);

						}
					}
				}
			}
		}
		mesh.SetVertices(vertices);
		mesh.SetTriangles(triangles, 0);
		mesh.SetUVs(0, uvs);
		mesh.SetNormals(normals);
		gameObject.SetActive(true);
		vertices.Clear();
		triangles.Clear();
		uvs.Clear();
		normals.Clear();
		meshCollider.sharedMesh = mesh;
	}

	private void AddFace(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 normal)
	{
		int index = vertices.Count;
		vertices.Add(a);
		vertices.Add(b);
		vertices.Add(c);
		vertices.Add(d);
		//uvs.Add(new Vector2(0, 0));
		//uvs.Add(new Vector2(0, 1));
		//uvs.Add(new Vector2(1, 1));
		//uvs.Add(new Vector2(1, 0));
		normals.Add(normal);
		normals.Add(normal);
		normals.Add(normal);
		normals.Add(normal);
		triangles.Add(index + 0);
		triangles.Add(index + 1);
		triangles.Add(index + 2);
		triangles.Add(index + 2);
		triangles.Add(index + 3);
		triangles.Add(index + 0);
	}

	private void AddTextureFace(TextureMapper.TextureMap.Face face)
	{
		uvs.Add(face.bl);
		uvs.Add(face.tl);
		uvs.Add(face.tr);
		uvs.Add(face.br);
	}

	public void Unload()
	{
		gameObject.SetActive(false);
		mesh.Clear();
	}
}