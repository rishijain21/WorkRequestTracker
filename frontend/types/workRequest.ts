export type Priority = 'Low' | 'Medium' | 'High';
export type WorkRequestStatus = 'New' | 'InProgress' | 'Blocked' | 'Completed';

export interface Note {
  id: string;
  content: string;
  createdAt: string;
}

export interface WorkRequestSummary {
  id: string;
  title: string;
  clientName: string;
  priority: Priority;
  status: WorkRequestStatus;
  dueDate: string;
  createdAt: string;
}

export interface WorkRequest extends WorkRequestSummary {
  description: string;
  updatedAt: string;
  notes: Note[];
}

export interface CreateWorkRequestPayload {
  title: string;
  clientName: string;
  description: string;
  priority: Priority;
  status: WorkRequestStatus;
  dueDate: string;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  errors?: string[];
  pagination?: {
    page: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
  };
}
