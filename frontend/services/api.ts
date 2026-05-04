import {
  ApiResponse,
  WorkRequestSummary,
  WorkRequest,
  CreateWorkRequestPayload,
  Note,
  WorkRequestStatus,
} from '@/types/workRequest';

const BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? 'http://localhost:5074';

async function request<T>(path: string, options?: RequestInit): Promise<ApiResponse<T>> {
  const res = await fetch(`${BASE_URL}${path}`, {
    headers: { 'Content-Type': 'application/json' },
    ...options,
  });

  const body = await res.json();

  // API always returns ApiResponse shape on success
  // On validation errors (ASP.NET model validation), shape differs
  if (!res.ok) {
    // Handle ASP.NET ProblemDetails format (model validation errors)
    if (body.errors && !body.success && body.title) {
      const messages = Object.values(body.errors).flat() as string[];
      throw new Error(messages.join(' '));
    }
    // Handle our custom ApiResponse error format
    throw new Error(body.message ?? 'Something went wrong');
  }

  return body;
}

export const workRequestApi = {
  getAll: (params: {
    status?: string;
    search?: string;
    page?: number;
    pageSize?: number;
  }): Promise<ApiResponse<WorkRequestSummary[]>> => {
    const query = new URLSearchParams(
      Object.entries(params)
        .filter(([, v]) => v != null && v !== '')
        .map(([k, v]) => [k, String(v)])
    );
    return request<WorkRequestSummary[]>(`/api/work-requests?${query}`);
  },

  getById: (id: string): Promise<ApiResponse<WorkRequest>> => {
    return request<WorkRequest>(`/api/work-requests/${id}`);
  },

  create: (payload: CreateWorkRequestPayload): Promise<ApiResponse<WorkRequest>> => {
    return request<WorkRequest>('/api/work-requests', {
      method: 'POST',
      body: JSON.stringify(payload),
    });
  },

  updateStatus: (id: string, status: WorkRequestStatus): Promise<ApiResponse<WorkRequest>> => {
    return request<WorkRequest>(`/api/work-requests/${id}/status`, {
      method: 'PATCH',
      body: JSON.stringify({ status }),
    });
  },

  addNote: (id: string, content: string): Promise<ApiResponse<Note>> => {
    return request<Note>(`/api/work-requests/${id}/notes`, {
      method: 'POST',
      body: JSON.stringify({ content }),
    });
  },
};
