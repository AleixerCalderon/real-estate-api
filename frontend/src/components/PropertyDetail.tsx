import { Property } from '@/types';
import { formatPrice } from '@/services/api';
import { MapPin, Calendar, User, Home, X, DollarSign, Building } from 'lucide-react';
import Image from 'next/image';

interface PropertyDetailProps {
  property: Property;
  onClose: () => void;
}

export function PropertyDetail({ property, onClose }: PropertyDetailProps) {
  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50 animate-fade-in">
      <div className="bg-white rounded-lg max-w-4xl w-full max-h-[90vh] overflow-y-auto animate-slide-up">
        <div className="relative h-80 w-full">
          <Image
            src={property.image || '/placeholder-property.jpg'}
            alt={property.name}
            fill
            className="object-cover rounded-t-lg"
          />
          <button
            onClick={onClose}
            className="absolute top-4 right-4 bg-white rounded-full p-3 shadow-lg hover:bg-gray-100 transition-colors"
          >
            <X className="h-5 w-5" />
          </button>
          <div className="absolute bottom-4 left-4 bg-black bg-opacity-70 text-white px-4 py-2 rounded-lg">
            <div className="text-2xl font-bold">{formatPrice(property.price)}</div>
          </div>
        </div>
        
        <div className="p-8">
          <div className="mb-6">
            <h1 className="text-3xl font-bold text-gray-900 mb-2">{property.name}</h1>
            <div className="flex items-center text-gray-600">
              <Building className="h-5 w-5 mr-2" />
              <span>Código: {property.codeInternal}</span>
            </div>
          </div>
          
          <div className="grid md:grid-cols-2 gap-8">
            <div>
              <h2 className="text-xl font-semibold text-gray-900 mb-4">Información General</h2>
              <div className="space-y-4">
                <div className="flex items-start gap-3">
                  <MapPin className="h-5 w-5 text-gray-400 mt-1 flex-shrink-0" />
                  <div>
                    <div className="font-medium text-gray-900">Ubicación</div>
                    <div className="text-gray-600">{property.address}</div>
                  </div>
                </div>
                
                <div className="flex items-start gap-3">
                  <Calendar className="h-5 w-5 text-gray-400 mt-1 flex-shrink-0" />
                  <div>
                    <div className="font-medium text-gray-900">Año de construcción</div>
                    <div className="text-gray-600">{property.year}</div>
                  </div>
                </div>
                
                <div className="flex items-start gap-3">
                  <DollarSign className="h-5 w-5 text-gray-400 mt-1 flex-shrink-0" />
                  <div>
                    <div className="font-medium text-gray-900">Precio</div>
                    <div className="text-2xl font-bold text-blue-600">{formatPrice(property.price)}</div>
                  </div>
                </div>
              </div>
            </div>
            
            <div>
              <h2 className="text-xl font-semibold text-gray-900 mb-4">Propietario</h2>
              <div className="bg-gray-50 rounded-lg p-4">
                <div className="flex items-center gap-3">
                  <User className="h-8 w-8 text-gray-400" />
                  <div>
                    <div className="font-medium text-gray-900">{property.ownerName}</div>
                    <div className="text-sm text-gray-600">Propietario registrado</div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          
          <div className="mt-8 pt-6 border-t border-gray-200 flex gap-4 justify-end">
            <button
              onClick={onClose}
              className="px-6 py-3 bg-gray-500 text-white rounded-lg hover:bg-gray-600 transition-colors font-medium"
            >
              Cerrar
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}