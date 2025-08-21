export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data?: T;
  total?: number;
  page?: number;
  pageSize?: number;
}

export interface PaginationInfo {
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export interface LoadingState {
  loading: boolean;
  error: string | null;
}