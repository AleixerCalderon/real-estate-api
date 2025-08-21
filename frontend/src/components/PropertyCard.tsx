import { Property } from '@/types';
import { formatPrice } from '@/services/api';
import { MapPin, Calendar, User, Eye, Edit, Trash2 } from 'lucide-react';
import Image from 'next/image';

interface PropertyCardProps {
  property: Property;
  onViewDetails: (id: string) => void;
  onEdit?: (property: Property) => void;
  onDelete?: (id: string) => void;
  showActions?: boolean;
}

export function PropertyCard({ 
  property, 
  onViewDetails, 
  onEdit, 
  onDelete, 
  showActions = false 
}: PropertyCardProps) {
  return (
    <div className="bg-white rounded-lg shadow-md hover:shadow-lg transition-all duration-300 overflow-hidden group animate-fade-in">
      <div className="relative h-48 w-full overflow-hidden">
        <Image
          src={property.image || '/placeholder-property.jpg'}
          alt={property.name}
          fill
          className="object-cover group-hover:scale-105 transition-transform duration-300"
          onError={(e) => {
            const target = e.target as HTMLImageElement;
            target.src = '/placeholder-property.jpg';
          }}
        />
        <div className="absolute top-2 right-2 bg-blue-600 text-white px-3 py-1 rounded-full text-sm font-semibold shadow-lg">
          {formatPrice(property.price)}
        </div>
        <div className="absolute bottom-2 left-2 bg-black bg-opacity-50 text-white px-2 py-1 rounded text-xs">
          Año {property.year}
        </div>
      </div>
      
      <div className="p-4">
        <h3 className="text-lg font-semibold text-gray-900 mb-2 line-clamp-2 hover:text-blue-600 transition-colors">
          {property.name}
        </h3>
        
        <div className="space-y-2 text-sm text-gray-600 mb-4">
          <div className="flex items-center gap-2">
            <MapPin className="h-4 w-4 text-gray-400 flex-shrink-0" />
            <span className="line-clamp-1">{property.address}</span>
          </div>
          
          <div className="flex items-center gap-2">
            <User className="h-4 w-4 text-gray-400 flex-shrink-0" />
            <span className="truncate">Propietario: {property.ownerName}</span>
          </div>
          
          <div className="flex items-center gap-2">
            <Calendar className="h-4 w-4 text-gray-400 flex-shrink-0" />
            <span>Construido en {property.year}</span>
          </div>
        </div>
        
        <div className="flex items-center justify-between">
          <span className="text-xs text-gray-500 bg-gray-100 px-2 py-1 rounded">
            Código: {property.codeInternal}
          </span>
          
          <div className="flex items-center gap-2">
            <button
              onClick={() => onViewDetails(property.id)}
              className="inline-flex items-center gap-1 bg-blue-600 text-white px-3 py-2 rounded-md text-sm font-medium hover:bg-blue-700 transition-colors"
            >
              <Eye className="h-4 w-4" />
              Ver
            </button>
            
            {showActions && (
              <>
                {onEdit && (
                  <button
                    onClick={() => onEdit(property)}
                    className="inline-flex items-center gap-1 bg-green-600 text-white px-3 py-2 rounded-md text-sm font-medium hover:bg-green-700 transition-colors"
                  >
                    <Edit className="h-4 w-4" />
                  </button>
                )}
                {onDelete && (
                  <button
                    onClick={() => onDelete(property.id)}
                    className="inline-flex items-center gap-1 bg-red-600 text-white px-3 py-2 rounded-md text-sm font-medium hover:bg-red-700 transition-colors"
                  >
                    <Trash2 className="h-4 w-4" />
                  </button>
                )}
              </>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}