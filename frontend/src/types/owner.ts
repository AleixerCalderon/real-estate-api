export interface Owner {
  id: string;
  name: string;
  address: string;
  phone: string;
  birthday: string;
}

export interface OwnerCreate {
  name: string;
  address: string;
  phone: string;
  birthday: string;
}

export interface OwnerUpdate {
  name?: string;
  address?: string;
  phone?: string;
  birthday?: string;
}