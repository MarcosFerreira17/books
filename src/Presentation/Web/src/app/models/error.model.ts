export interface ProblemDetails {
  type?: string;
  title?: string;
  status?: number;
  detail?: string;
  errors?: { [key: string]: string[] };
  instance?: string;
}

export type BackendError = string[] | ProblemDetails;
