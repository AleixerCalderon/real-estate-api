export interface Property {
  id: string;
  idOwner: string;
  name: string;
  address: string;
  price: number;
  image: string;
  ownerName: string;
  year: number;
  codeInternal: number;
}

export interface PropertyFilter {
  name?: string;
  address?: string;
  minPrice?: number;
  maxPrice?: number;
  page?: number;
  pageSize?: number;
}

export interface PropertyCreate {
  name: string;
  address: string;
  price: number;
  idOwner: string;
  image: string;
  year: number;
}

export interface PropertyUpdate {
  name?: string;
  address?: string;
  price?: number;
  image?: string;
  year?: number;
}

